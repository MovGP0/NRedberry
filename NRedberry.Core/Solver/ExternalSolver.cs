using System.Diagnostics;
using NRedberry.Contexts;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;
using NRedberry.Transformations.Substitutions;
using TensorCC = NRedberry.Tensors.CC;
using TensorOps = NRedberry.Tensors.Tensors;

namespace NRedberry.Solver;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/ExternalSolver.java
 */

public static class ExternalSolver
{
    public static Expression[][] SolveSystemWithMaple(
        ReducedSystem reducedSystem,
        bool keepFreeParameters,
        string mapleBinDir,
        string path)
    {
        return SolveSystemWithExternalProgram(MapleScriptCreator.Instance, reducedSystem, keepFreeParameters, mapleBinDir, path);
    }

    public static Expression[][] SolveSystemWithMathematica(
        ReducedSystem reducedSystem,
        bool keepFreeParameters,
        string mathematicaBinDir,
        string path)
    {
        return SolveSystemWithExternalProgram(MathematicaScriptCreator.Instance, reducedSystem, keepFreeParameters, mathematicaBinDir, path);
    }

    public static Expression[][] SolveSystemWithExternalProgram(
        ExternalScriptCreator scriptCreator,
        ReducedSystem reducedSystem,
        bool keepFreeParameters,
        string programBinDir,
        string path)
    {
        ArgumentNullException.ThrowIfNull(scriptCreator);
        ArgumentNullException.ThrowIfNull(reducedSystem);
        ArgumentNullException.ThrowIfNull(programBinDir);
        ArgumentNullException.ThrowIfNull(path);

        Expression[] equations = (Expression[])reducedSystem.GetEquations().Clone();
        var tensorSubstitutions = new Dictionary<Tensor, Tensor>();

        for (int i = 0; i < equations.Length; ++i)
        {
            Expression eq = equations[i];
            var iterator = new FromChildToParentIterator(eq);
            Tensor t;
            while ((t = iterator.Next()) is not null)
            {
                if (t is not Product product || t.Indices.Size() == 0)
                {
                    continue;
                }

                Tensor[] scalars = product.Content.Scalars;
                foreach (Tensor scalar in scalars)
                {
                    if (!tensorSubstitutions.ContainsKey(scalar))
                    {
                        tensorSubstitutions[scalar] = TensorCC.GenerateNewSymbol();
                    }
                }
            }
        }

        var scalarSubs = new Expression[tensorSubstitutions.Count];
        int index = -1;
        foreach (var entry in tensorSubstitutions)
        {
            scalarSubs[++index] = TensorOps.Expression(entry.Key, entry.Value);
        }

        var fullSub = new SubstitutionTransformation(scalarSubs, true);
        for (int i = 0; i < equations.Length; ++i)
        {
            equations[i] = (Expression)fullSub.Transform(equations[i]);
        }

        scriptCreator.CreateScript(equations, reducedSystem, path, keepFreeParameters);

        string outputFilePath = Path.Combine(path, $"equations.{scriptCreator.GetScriptExtension()}Out");
        if (File.Exists(outputFilePath))
        {
            File.Delete(outputFilePath);
        }

        try
        {
            string scriptPath = Path.Combine(path, $"equations.{scriptCreator.GetScriptExtension()}");
            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(programBinDir, scriptCreator.GetScriptExecutionCommand()),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            foreach (string parameter in scriptCreator.GetParameters())
            {
                startInfo.ArgumentList.Add(parameter);
            }

            startInfo.ArgumentList.Add(scriptPath);

            using Process process = new() { StartInfo = startInfo };
            process.Start();

            string? line;
            while ((line = process.StandardOutput.ReadLine()) is not null)
            {
                Console.WriteLine(line);
            }

            while ((line = process.StandardError.ReadLine()) is not null)
            {
                Console.WriteLine(line);
            }

            process.WaitForExit();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("External solver execution failed.", ex);
        }

        var coefficientsResults = new Expression[reducedSystem.GetUnknownCoefficients().Length];
        if (!File.Exists(outputFilePath))
        {
            return Array.Empty<Expression[]>();
        }

        if (new FileInfo(outputFilePath).Length == 0)
        {
            return null;
        }

        List<Expression[]> solutions = [];
        using (var reader = new StreamReader(outputFilePath))
        {
            string? strLine;
            int i = -1;
            while ((strLine = reader.ReadLine()) is not null)
            {
                if (strLine == "//solution")
                {
                    Expression[] solution = (Expression[])reducedSystem.GetGeneralSolutions().Clone();
                    List<ITransformation> zeroSubs = [];
                    foreach (Expression coef in coefficientsResults)
                    {
                        if (coef.IsIdentity() && !keepFreeParameters)
                        {
                            zeroSubs.Add(TensorOps.Expression(coef[0], Complex.Zero));
                        }
                        else
                        {
                            for (int si = solution.Length - 1; si >= 0; --si)
                            {
                                solution[si] = (Expression)coef.Transform(solution[si]);
                            }
                        }
                    }

                    if (!keepFreeParameters)
                    {
                        var zeroSubstitution = new TransformationCollection(zeroSubs);
                        for (int si = solution.Length - 1; si >= 0; --si)
                        {
                            solution[si] = (Expression)zeroSubstitution.Transform(solution[si]);
                        }
                    }

                    foreach (Expression sub in scalarSubs)
                    {
                        for (int si = solution.Length - 1; si >= 0; --si)
                        {
                            solution[si] = (Expression)sub.Transpose().Transform(solution[si]);
                        }
                    }

                    solutions.Add(solution);
                    coefficientsResults = new Expression[reducedSystem.GetUnknownCoefficients().Length];
                    i = -1;
                }
                else
                {
                    coefficientsResults[++i] = TensorOps.ParseExpression(strLine);
                }
            }
        }

        return solutions.ToArray();
    }

    public interface ExternalScriptCreator
    {
        void CreateScript(Expression[] equations, ReducedSystem reducedSystem, string path, bool keepFreeParams);

        string GetScriptExecutionCommand();

        string GetScriptExtension();

        string[] GetParameters();
    }

    public sealed class MathematicaScriptCreator : ExternalScriptCreator
    {
        public static MathematicaScriptCreator Instance { get; } = new();

        private MathematicaScriptCreator()
        {
        }

        public void CreateScript(Expression[] equations, ReducedSystem reducedSystem, string path, bool keepFreeParams)
        {
            string scriptPath = Path.Combine(path, "equations.mathematica");
            using var file = new StreamWriter(scriptPath, false);

            file.WriteLine("$equations = {");
            for (int i = 0; ; i++)
            {
                file.Write(equations[i].ToString(OutputFormat.WolframMathematica).Replace("=", "==", StringComparison.Ordinal));
                if (i == equations.Length - 1)
                {
                    break;
                }

                file.WriteLine(",");
            }

            file.WriteLine();
            file.WriteLine("};");

            SimpleTensor[] coefficients = reducedSystem.GetUnknownCoefficients();
            file.Write("$coefficients = {");
            for (int i = 0; ; i++)
            {
                file.Write(coefficients[i].ToString(OutputFormat.WolframMathematica));
                if (i == coefficients.Length - 1)
                {
                    break;
                }

                file.Write(',');
            }

            file.WriteLine(" };");

            file.WriteLine("$result = Solve[$equations,$coefficients];");
            file.Write("If[Length[$result] != 0, ");
            file.WriteLine("$result = Simplify[$result];");
            file.WriteLine($"$stream = OpenWrite[\"{path}/equations.mathematicaOut\"];");
            file.Write("For[$solution = 1, $solution <= Length[$result], ++$solution, ");
            file.Write("$tempResult = $result[[$solution]];");
            file.WriteLine("$found = $tempResult[[All, 1]];");
            if (!keepFreeParams)
            {
                file.WriteLine("For[$i = 1, $i <= Length[$coefficients], ++$i,If[!MemberQ[$found, $coefficients[[$i]]], $tempResult = $tempResult/.{$coefficients[[$i]] -> 0}; AppendTo[$tempResult, $coefficients[[$i]] -> 0];]];");
            }
            else
            {
                file.WriteLine("For[$i = 1, $i <= Length[$coefficients], ++$i,If[!MemberQ[$found, $coefficients[[$i]]], AppendTo[$tempResult, $coefficients[[$i]] -> $coefficients[[$i]]];]];");
            }

            file.WriteLine("$tempResult = Simplify[$tempResult];");
            file.WriteLine("For[$i = 1, $i <= Length[$coefficients], ++$i, WriteString[$stream, StringReplace[ToString[$tempResult[[$i]] // InputForm], {\"->\" -> \"=\", \"^\" -> \"**\"}] <> If[$i != Length[$coefficients], \"\\n\", \"\"]]];");
            file.WriteLine("WriteString[$stream, \"\\n//solution\\n\"];");
            file.WriteLine("];");
            file.WriteLine("Close[$stream];");
            file.WriteLine("];");
        }

        public string GetScriptExecutionCommand()
        {
            return "MathematicaScript";
        }

        public string GetScriptExtension()
        {
            return "mathematica";
        }

        public string[] GetParameters()
        {
            return new[] { "-script" };
        }
    }

    public sealed class MapleScriptCreator : ExternalScriptCreator
    {
        public static MapleScriptCreator Instance { get; } = new();

        private MapleScriptCreator()
        {
        }

        public void CreateScript(Expression[] equations, ReducedSystem reducedSystem, string path, bool keepFreeParams)
        {
            string scriptPath = Path.Combine(path, "equations.maple");
            using var file = new StreamWriter(scriptPath, false);

            SimpleTensor[] coefficients = reducedSystem.GetUnknownCoefficients();
            file.WriteLine("with(StringTools):");
            file.Write("ans:=array([");
            for (int i = 0; i < coefficients.Length; ++i)
            {
                if (i == coefficients.Length - 1)
                {
                    file.Write(coefficients[i].ToString(OutputFormat.Maple));
                }
                else
                {
                    file.Write(coefficients[i].ToString(OutputFormat.Maple) + ",");
                }
            }

            file.WriteLine("]):");
            file.WriteLine($"eq:=array(1..{equations.Length}):");
            for (int i = 0; i < equations.Length; i++)
            {
                file.WriteLine($"eq[{i + 1}]:={equations[i].ToString(OutputFormat.Maple)}:");
            }

            file.Write("Result := solve(simplify({seq(eq[i],i=1.." + equations.Length + ")}),[");
            for (int i = 0; i < coefficients.Length; ++i)
            {
                if (i == coefficients.Length - 1)
                {
                    file.Write(coefficients[i].ToString(OutputFormat.Maple));
                }
                else
                {
                    file.Write(coefficients[i].ToString(OutputFormat.Maple) + ",");
                }
            }

            file.WriteLine("],explicit=true):");
            file.WriteLine("if nops(Result) <> 0 then");
            file.WriteLine("Result:= factor(Result);");
            file.WriteLine($"file:=fopen(\"{path}/equations.mapleOut\",WRITE):");
            file.WriteLine("for maple_positionInResult from 1 to nops(Result) do");
            file.WriteLine("for maple_counter from 1 to " + coefficients.Length + " do");
            file.WriteLine("temp1 := SubstituteAll(convert(lhs(Result[maple_positionInResult][maple_counter]), string), \"^\", \"**\");");
            file.WriteLine("temp1 := SubstituteAll(convert(lhs(Result[maple_positionInResult][maple_counter]), string), \"(\", \"[\");");
            file.WriteLine("temp1 := SubstituteAll(convert(lhs(Result[maple_positionInResult][maple_counter]), string), \")\", \"]\");");
            file.WriteLine("temp2 := SubstituteAll(convert(rhs(Result[maple_positionInResult][maple_counter]), string), \"^\", \"**\");");
            file.WriteLine("fprintf(file,\"%s=%s\\n\",temp1,temp2);");
            file.WriteLine("od:");
            file.WriteLine("fprintf(file,\"//solution\\n\");");
            file.WriteLine("od:");
            file.WriteLine("end if;");
            file.WriteLine("fclose(file):");
        }

        public string GetScriptExecutionCommand()
        {
            return "maple";
        }

        public string GetScriptExtension()
        {
            return "maple";
        }

        public string[] GetParameters()
        {
            return Array.Empty<string>();
        }
    }
}
