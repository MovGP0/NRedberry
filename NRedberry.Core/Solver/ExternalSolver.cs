using NRedberry.Tensors;

namespace NRedberry.Solver;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/ExternalSolver.java
 */

public static class ExternalSolver
{
    public static Expression[][] SolveSystemWithMaple(ReducedSystem reducedSystem, bool keepFreeParameters, string mapleBinDir, string path)
    {
        throw new NotImplementedException();
    }

    public static Expression[][] SolveSystemWithMathematica(ReducedSystem reducedSystem, bool keepFreeParameters, string mathematicaBinDir, string path)
    {
        throw new NotImplementedException();
    }

    public static Expression[][] SolveSystemWithExternalProgram(ExternalScriptCreator scriptCreator, ReducedSystem reducedSystem, bool keepFreeParameters, string programBinDir, string path)
    {
        throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public string GetScriptExecutionCommand()
        {
            throw new NotImplementedException();
        }

        public string[] GetParameters()
        {
            throw new NotImplementedException();
        }

        public string GetScriptExtension()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public string GetScriptExecutionCommand()
        {
            throw new NotImplementedException();
        }

        public string GetScriptExtension()
        {
            throw new NotImplementedException();
        }

        public string[] GetParameters()
        {
            throw new NotImplementedException();
        }
    }
}
