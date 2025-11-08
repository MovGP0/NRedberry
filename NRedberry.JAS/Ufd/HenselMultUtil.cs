using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Hensel multivariate lifting utilities.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.HenselMultUtil
/// </remarks>
public static class HenselMultUtil
{
    private const bool Debug = false;

    /// <summary>
    /// Modular diophantine equation solution and lifting algorithm.
    /// </summary>
    /// <typeparam name="MOD">coefficient type</typeparam>
    /// <param name="A">modular polynomial, modulo p^k</param>
    /// <param name="B">modular polynomial, modulo p^k</param>
    /// <param name="C">modular polynomial, modulo p^k</param>
    /// <param name="substitutionValues">list of substitution values</param>
    /// <param name="d">desired approximation exponent for (x_i - v_i)^d</param>
    /// <param name="k">desired approximation exponent for p^k</param>
    /// <returns>List [s, t] with s·A' + t·B' = C modulo p^k, where A' = B and B' = A.</returns>
    /// <exception cref="NoLiftingException">Thrown when the lifting fails.</exception>
    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        GenPolynomial<MOD> A,
        GenPolynomial<MOD> B,
        GenPolynomial<MOD> C,
        List<MOD> substitutionValues,
        long d,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(A);
        ArgumentNullException.ThrowIfNull(B);
        ArgumentNullException.ThrowIfNull(C);
        ArgumentNullException.ThrowIfNull(substitutionValues);

        GenPolynomialRing<MOD> pkfac = C.Ring;
        if (pkfac.Nvar == 1)
        {
            return HenselUtil.LiftDiophant(A, B, C, k);
        }

        if (!pkfac.Equals(A.Ring))
        {
            throw new ArgumentException($"A.Ring != pkfac: {A.Ring} != {pkfac}", nameof(A));
        }

        List<MOD> remainingValues = new(substitutionValues);
        MOD v = remainingValues[remainingValues.Count - 1];
        remainingValues.RemoveAt(remainingValues.Count - 1);

        GenPolynomial<MOD> mon = pkfac.FromInteger(1);
        GenPolynomial<MOD> xv = pkfac.Univariate(0, 1);
        xv = xv.Subtract(pkfac.FromInteger(v.GetSymmetricInteger().Val));

        ModularRingFactory<MOD> coefficientFactory = (ModularRingFactory<MOD>)pkfac.CoFac;
        MOD vp = coefficientFactory.FromInteger(v.GetSymmetricInteger().Val);
        GenPolynomialRing<MOD> contractedRing = pkfac.Contract(1);
        GenPolynomial<MOD> Ap = PolyUtil.EvaluateMain(contractedRing, A, vp);
        GenPolynomial<MOD> Bp = PolyUtil.EvaluateMain(contractedRing, B, vp);
        GenPolynomial<MOD> Cp = PolyUtil.EvaluateMain(contractedRing, C, vp);

        List<GenPolynomial<MOD>> su = LiftDiophant(Ap, Bp, Cp, remainingValues, d, k);
        if (pkfac.Nvar == 2 && !HenselUtil.IsDiophantLift(Bp, Ap, su[0], su[1], Cp))
        {
        }

        if (!contractedRing.Equals(su[0].Ring))
        {
            throw new ArgumentException($"coefficient ring mismatch: {su[0].Ring} != {contractedRing}");
        }

        GenPolynomialRing<BigInteger> integerRing = HenselUtil.CreateIntegerPolynomialRing(pkfac);
        GenPolynomialRing<GenPolynomial<MOD>> recursiveRing = contractedRing.Recursive(1);

        List<GenPolynomial<MOD>> sup = new(su.Count);
        List<GenPolynomial<BigInteger>> supInteger = new(su.Count);
        foreach (GenPolynomial<MOD> s in su)
        {
            GenPolynomial<MOD> extended = s.Extend(pkfac, 0, 0L);
            sup.Add(extended);
            supInteger.Add(PolyUtil.IntegerFromModularCoefficients(integerRing, extended));
        }

        GenPolynomial<BigInteger> Ai = PolyUtil.IntegerFromModularCoefficients(integerRing, A);
        GenPolynomial<BigInteger> Bi = PolyUtil.IntegerFromModularCoefficients(integerRing, B);
        GenPolynomial<BigInteger> Ci = PolyUtil.IntegerFromModularCoefficients(integerRing, C);

        GenPolynomial<BigInteger> error = Ci;
        error = error.Subtract(Bi.Multiply(supInteger[0]));
        error = error.Subtract(Ai.Multiply(supInteger[1]));
        if (error.IsZero())
        {
            return sup;
        }

        GenPolynomial<MOD> modularError = PolyUtil.FromIntegerCoefficients(pkfac, error);
        if (modularError.IsZero())
        {
            return sup;
        }

        for (int e = 1; e <= d; e++)
        {
            GenPolynomial<GenPolynomial<MOD>> recursiveError = PolyUtil.Recursive(recursiveRing, modularError);
            UnivPowerSeriesRing<GenPolynomial<MOD>> seriesRing = new(recursiveRing);
            TaylorFunction<GenPolynomial<MOD>> taylor = new PolynomialTaylorFunction<GenPolynomial<MOD>>(recursiveError);
            GenPolynomial<MOD> evaluationPoint = contractedRing.FromInteger(v.GetSymmetricInteger().Val);
            UnivPowerSeries<GenPolynomial<MOD>> series = seriesRing.SeriesOfTaylor(taylor, evaluationPoint);
            GenPolynomial<MOD> coefficient = series.Coefficient(e);
            if (coefficient.IsZero())
            {
                continue;
            }

            List<GenPolynomial<MOD>> correction = LiftDiophant(Ap, Bp, coefficient, remainingValues, d, k);
            if (!contractedRing.CoFac.Equals(correction[0].Ring.CoFac))
            {
                throw new ArgumentException($"coefficient factory mismatch: {contractedRing.CoFac} != {correction[0].Ring.CoFac}");
            }

            if (pkfac.Nvar == 2 && !HenselUtil.IsDiophantLift(Ap, Bp, correction[1], correction[0], coefficient))
            {
            }

            mon = mon.Multiply(xv);
            List<GenPolynomial<BigInteger>> updatedInteger = new(correction.Count);
            for (int i = 0; i < correction.Count; i++)
            {
                GenPolynomial<MOD> extended = correction[i].Extend(pkfac, 0, 0L);
                GenPolynomial<MOD> scaled = extended.Multiply(mon);
                GenPolynomial<MOD> sum = sup[i].Sum(scaled);
                sup[i] = sum;
                updatedInteger.Add(PolyUtil.IntegerFromModularCoefficients(integerRing, scaled));
            }

            for (int i = 0; i < updatedInteger.Count; i++)
            {
                error = error.Subtract((i == 0 ? Bi : Ai).Multiply(updatedInteger[i]));
            }

            if (error.IsZero())
            {
                return sup;
            }

            modularError = PolyUtil.FromIntegerCoefficients(pkfac, error);
            if (modularError.IsZero())
            {
                return sup;
            }
        }

        return sup;
    }

    /// <summary>
    /// Modular diophantine equation solution and lifting algorithm for several polynomials.
    /// </summary>
    public static List<GenPolynomial<MOD>> LiftDiophant<MOD>(
        List<GenPolynomial<MOD>> A,
        GenPolynomial<MOD> C,
        List<MOD> substitutionValues,
        long d,
        long k)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(A);
        ArgumentNullException.ThrowIfNull(C);
        ArgumentNullException.ThrowIfNull(substitutionValues);
        if (A.Count == 0)
        {
            throw new ArgumentException("List of polynomials must be non-empty.", nameof(A));
        }

        GenPolynomialRing<MOD> pkfac = C.Ring;
        if (pkfac.Nvar == 1)
        {
            return HenselUtil.LiftDiophant(A, C, k);
        }

        if (!pkfac.Equals(A[0].Ring))
        {
            throw new ArgumentException($"A[0].Ring != pkfac: {A[0].Ring} != {pkfac}", nameof(A));
        }

        GenPolynomial<MOD> As = pkfac.FromInteger(1);
        foreach (GenPolynomial<MOD> polynomial in A)
        {
            As = As.Multiply(polynomial);
        }

        List<GenPolynomial<MOD>> coproducts = new(A.Count);
        foreach (GenPolynomial<MOD> polynomial in A)
        {
            GenPolynomial<MOD> cofactor = PolyUtil.BasePseudoDivide(As, polynomial);
            coproducts.Add(cofactor);
        }

        List<MOD> remainingValues = new(substitutionValues);
        MOD v = remainingValues[remainingValues.Count - 1];
        remainingValues.RemoveAt(remainingValues.Count - 1);

        GenPolynomial<MOD> mon = pkfac.FromInteger(1);
        GenPolynomial<MOD> xv = pkfac.Univariate(0, 1);
        xv = xv.Subtract(pkfac.FromInteger(v.GetSymmetricInteger().Val));

        ModularRingFactory<MOD> coefficientFactory = (ModularRingFactory<MOD>)pkfac.CoFac;
        MOD vp = coefficientFactory.FromInteger(v.GetSymmetricInteger().Val);
        GenPolynomialRing<MOD> contractedRing = pkfac.Contract(1);

        List<GenPolynomial<MOD>> evaluatedA = new(A.Count);
        foreach (GenPolynomial<MOD> polynomial in A)
        {
            evaluatedA.Add(PolyUtil.EvaluateMain(contractedRing, polynomial, vp));
        }

        GenPolynomial<MOD> evaluatedC = PolyUtil.EvaluateMain(contractedRing, C, vp);

        List<GenPolynomial<MOD>> su = LiftDiophant(evaluatedA, evaluatedC, remainingValues, d, k);
        if (pkfac.Nvar == 2 && !HenselUtil.IsDiophantLift(evaluatedA, su, evaluatedC))
        {
        }

        if (!contractedRing.Equals(su[0].Ring))
        {
            throw new ArgumentException($"coefficient ring mismatch: {su[0].Ring} != {contractedRing}");
        }

        GenPolynomialRing<BigInteger> integerRing = HenselUtil.CreateIntegerPolynomialRing(pkfac);
        GenPolynomialRing<GenPolynomial<MOD>> recursiveRing = contractedRing.Recursive(1);

        List<GenPolynomial<MOD>> sup = new(su.Count);
        List<GenPolynomial<BigInteger>> supInteger = new(su.Count);
        foreach (GenPolynomial<MOD> s in su)
        {
            GenPolynomial<MOD> extended = s.Extend(pkfac, 0, 0L);
            sup.Add(extended);
            supInteger.Add(PolyUtil.IntegerFromModularCoefficients(integerRing, extended));
        }

        List<GenPolynomial<BigInteger>> Ai = PolyUtil.IntegerFromModularCoefficients(integerRing, A);
        List<GenPolynomial<BigInteger>> Bi = PolyUtil.IntegerFromModularCoefficients(integerRing, coproducts);
        GenPolynomial<BigInteger> Ci = PolyUtil.IntegerFromModularCoefficients(integerRing, C);

        GenPolynomial<BigInteger> error = Ci;
        for (int i = 0; i < Bi.Count; i++)
        {
            error = error.Subtract(Bi[i].Multiply(supInteger[i]));
        }

        if (error.IsZero())
        {
            return sup;
        }

        GenPolynomial<MOD> modularError = PolyUtil.FromIntegerCoefficients(pkfac, error);
        if (modularError.IsZero())
        {
            return sup;
        }

        for (int e = 1; e <= d; e++)
        {
            GenPolynomial<GenPolynomial<MOD>> recursiveError = PolyUtil.Recursive(recursiveRing, modularError);
            UnivPowerSeriesRing<GenPolynomial<MOD>> seriesRing = new(recursiveRing);
            TaylorFunction<GenPolynomial<MOD>> taylor = new PolynomialTaylorFunction<GenPolynomial<MOD>>(recursiveError);
            GenPolynomial<MOD> evaluationPoint = contractedRing.FromInteger(v.GetSymmetricInteger().Val);
            UnivPowerSeries<GenPolynomial<MOD>> series = seriesRing.SeriesOfTaylor(taylor, evaluationPoint);
            GenPolynomial<MOD> coefficient = series.Coefficient(e);
            if (coefficient.IsZero())
            {
                continue;
            }

            List<GenPolynomial<MOD>> correction = LiftDiophant(evaluatedA, coefficient, remainingValues, d, k);
            if (!contractedRing.CoFac.Equals(correction[0].Ring.CoFac))
            {
                throw new ArgumentException($"coefficient factory mismatch: {contractedRing.CoFac} != {correction[0].Ring.CoFac}");
            }

            if (pkfac.Nvar == 2 && !HenselUtil.IsDiophantLift(evaluatedA, correction, coefficient))
            {
            }

            mon = mon.Multiply(xv);
            List<GenPolynomial<BigInteger>> updatedInteger = new(correction.Count);
            for (int i = 0; i < correction.Count; i++)
            {
                GenPolynomial<MOD> extended = correction[i].Extend(pkfac, 0, 0L);
                GenPolynomial<MOD> scaled = extended.Multiply(mon);
                GenPolynomial<MOD> sum = sup[i].Sum(scaled);
                sup[i] = sum;
                updatedInteger.Add(PolyUtil.IntegerFromModularCoefficients(integerRing, scaled));
            }

            for (int i = 0; i < Bi.Count; i++)
            {
                error = error.Subtract(Bi[i].Multiply(updatedInteger[i]));
            }

            if (error.IsZero())
            {
                return sup;
            }

            modularError = PolyUtil.FromIntegerCoefficients(pkfac, error);
            if (modularError.IsZero())
            {
                return sup;
            }
        }

        return sup;
    }

    /// <summary>
    /// Modular multivariate Hensel lifting algorithm.
    /// </summary>
    public static List<GenPolynomial<MOD>> LiftHensel<MOD>(
        GenPolynomial<BigInteger> C,
        GenPolynomial<MOD> Cp,
        List<GenPolynomial<MOD>> F,
        List<MOD> substitutionValues,
        long k,
        List<GenPolynomial<BigInteger>> G)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(C);
        ArgumentNullException.ThrowIfNull(Cp);
        ArgumentNullException.ThrowIfNull(F);
        ArgumentNullException.ThrowIfNull(substitutionValues);
        ArgumentNullException.ThrowIfNull(G);
        if (F.Count == 0)
        {
            throw new ArgumentException("Factor list must be non-empty.", nameof(F));
        }

        GenPolynomialRing<MOD> pkfac = Cp.Ring;
        long deg = C.Degree();

        GenPolynomialRing<MOD> pkfacWithLower = HenselUtil.CreatePolynomialRingFromTemplate(pkfac.CoFac, G[0].Ring);
        List<GenPolynomial<MOD>> liftedLeading = new(G.Count);
        foreach (GenPolynomial<BigInteger> factor in G)
        {
            GenPolynomial<MOD> converted = PolyUtil.FromIntegerCoefficients(pkfacWithLower, factor)
                ?? throw new InvalidOperationException("Failed to convert integer coefficients.");
            converted = converted.ExtendLower(pkfac, 0, 0L);
            liftedLeading.Add(converted);
        }

        List<GenPolynomialRing<MOD>> ringStack = new();
        List<GenPolynomial<MOD>> polynomialStack = new();
        List<List<GenPolynomial<MOD>>> leadingStack = new();
        List<MOD> remainingValues = new();

        ringStack.Add(pkfac);
        polynomialStack.Add(Cp);
        leadingStack.Add(liftedLeading);

        GenPolynomialRing<MOD> currentRing = pkfac;
        GenPolynomial<MOD> currentPolynomial = Cp;
        List<GenPolynomial<MOD>> currentLeading = liftedLeading;

        for (int j = pkfac.Nvar; j > 2; j--)
        {
            currentRing = currentRing.Contract(1);
            ringStack.Insert(0, currentRing);

            MOD value = pkfac.CoFac.FromInteger(substitutionValues[pkfac.Nvar - j].GetSymmetricInteger().Val);
            remainingValues.Add(value);

            currentPolynomial = PolyUtil.EvaluateMain(currentRing, currentPolynomial, value);
            polynomialStack.Insert(0, currentPolynomial);

            List<GenPolynomial<MOD>> evaluatedLeading = new(currentLeading.Count);
            foreach (GenPolynomial<MOD> leading in currentLeading)
            {
                evaluatedLeading.Add(PolyUtil.EvaluateMain(currentRing, leading, value));
            }

            currentLeading = evaluatedLeading;
            leadingStack.Insert(0, currentLeading);
        }

        remainingValues.Add(substitutionValues[pkfac.Nvar - 2]);

        if (Debug)
        {
        }

        if (!pkfac.CoFac.Equals(F[0].Ring.CoFac))
        {
            throw new ArgumentException($"Factor ring mismatch: {F[0].Ring} != {pkfac}", nameof(F));
        }

        List<GenPolynomial<MOD>> resultFactors = new(F);
        GenPolynomial<BigInteger> error = new(C.Ring);
        List<MOD> evaluatedValues = new();
        List<GenPolynomial<BigInteger>>? Si = null;

        while (ringStack.Count > 0)
        {
            pkfac = ringStack[0];
            ringStack.RemoveAt(0);
            Cp = polynomialStack[0];
            polynomialStack.RemoveAt(0);
            currentLeading = leadingStack[0];
            leadingStack.RemoveAt(0);
            MOD v = remainingValues[remainingValues.Count - 1];
            remainingValues.RemoveAt(remainingValues.Count - 1);

            List<GenPolynomial<MOD>> previousFactors = resultFactors;
            resultFactors = new List<GenPolynomial<MOD>>(previousFactors.Count);

            for (int j = 0; j < previousFactors.Count; j++)
            {
                GenPolynomial<MOD> lifted = previousFactors[j].Extend(pkfac, 0, 0L);
                GenPolynomial<MOD> leading = currentLeading[j];
                if (!leading.IsOne())
                {
                    GenPolynomialRing<GenPolynomial<MOD>> recursive = pkfac.Recursive(pkfac.Nvar - 1);
                    GenPolynomial<GenPolynomial<MOD>> recursivePoly = PolyUtil.Recursive(recursive, lifted);
                    GenPolynomial<GenPolynomial<MOD>> switched = PolyUtil.SwitchVariables(recursivePoly);
                    GenPolynomial<GenPolynomial<MOD>> leadingRecursive = PolyUtil.Recursive(recursive, leading);
                    GenPolynomial<GenPolynomial<MOD>> leadingSwitched = PolyUtil.SwitchVariables(leadingRecursive);
                    if (!leadingSwitched.IsConstant())
                    {
                        throw new InvalidOperationException("Leading coefficient is not constant.");
                    }

                    leadingSwitched.DoPutToMap(switched.LeadingExpVector(), leadingSwitched.LeadingBaseCoefficient());
                    GenPolynomial<GenPolynomial<MOD>> backSwitched = PolyUtil.SwitchVariables(switched);
                    lifted = PolyUtil.Distribute(pkfac, backSwitched);
                }

                resultFactors.Add(lifted);
            }

            GenPolynomial<MOD> mon = pkfac.FromInteger(1);
            GenPolynomial<MOD> xv = pkfac.Univariate(0, 1).Subtract(pkfac.FromInteger(v.GetSymmetricInteger().Val));
            long currentDegree = Cp.Degree(pkfac.Nvar - 1);

            GenPolynomialRing<BigInteger> integerRing = HenselUtil.CreateIntegerPolynomialRing(pkfac);
            List<GenPolynomial<BigInteger>> integerFactors = PolyUtil.IntegerFromModularCoefficients(integerRing, resultFactors);
            GenPolynomial<BigInteger> integerCp = PolyUtil.IntegerFromModularCoefficients(integerRing, Cp);

            error = integerRing.FromInteger(1);
            foreach (GenPolynomial<BigInteger> factor in integerFactors)
            {
                error = error.Multiply(factor);
            }

            error = integerCp.Subtract(error);
            GenPolynomial<MOD> modularError = PolyUtil.FromIntegerCoefficients(pkfac, error);

            GenPolynomialRing<GenPolynomial<MOD>> recursiveRing = pkfac.Recursive(1);
            GenPolynomialRing<MOD> coefficientRing = (GenPolynomialRing<MOD>)recursiveRing.CoFac;

            for (int e = 1; e <= currentDegree && !modularError.IsZero(); e++)
            {
                GenPolynomial<GenPolynomial<MOD>> recursiveError = PolyUtil.Recursive(recursiveRing, modularError);
                UnivPowerSeriesRing<GenPolynomial<MOD>> seriesRing = new(recursiveRing);
                TaylorFunction<GenPolynomial<MOD>> taylor = new PolynomialTaylorFunction<GenPolynomial<MOD>>(recursiveError);
                GenPolynomial<MOD> evaluationPoint = coefficientRing.FromInteger(v.GetSymmetricInteger().Val);
                UnivPowerSeries<GenPolynomial<MOD>> series = seriesRing.SeriesOfTaylor(taylor, evaluationPoint);
                GenPolynomial<MOD> coefficient = series.Coefficient(e);
                if (coefficient.IsZero())
                {
                    continue;
                }

                List<GenPolynomial<MOD>> corrections = LiftDiophant(previousFactors, coefficient, evaluatedValues, deg, k);
                mon = mon.Multiply(xv);

                Si = new List<GenPolynomial<BigInteger>>(corrections.Count);
                for (int i = 0; i < corrections.Count; i++)
                {
                    GenPolynomial<MOD> extended = corrections[i].Extend(pkfac, 0, 0L);
                    GenPolynomial<MOD> scaled = extended.Multiply(mon);
                    GenPolynomial<MOD> sum = resultFactors[i].Sum(scaled);
                    resultFactors[i] = sum;
                    Si.Add(PolyUtil.IntegerFromModularCoefficients(integerRing, sum));
                }

                error = integerRing.FromInteger(1);
                foreach (GenPolynomial<BigInteger> factor in Si)
                {
                    error = error.Multiply(factor);
                }

                error = integerCp.Subtract(error);
                modularError = PolyUtil.FromIntegerCoefficients(pkfac, error);
            }

            evaluatedValues.Add(v);
            GenPolynomial<MOD> product = resultFactors[0].Ring.FromInteger(1);
            foreach (GenPolynomial<MOD> factor in resultFactors)
            {
                product = product.Multiply(factor);
            }

            if (Debug && !modularError.IsZero())
            {
            }
        }

        if (error.IsZero())
        {
        }

        return resultFactors;
    }
}
