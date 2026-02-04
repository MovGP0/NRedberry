using System.Diagnostics;
using System.IO;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Factor;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Port of cc.redberry.physics.oneloopdiv.Benchmarks.
/// </summary>
public sealed class Benchmarks
{
    private static readonly Stream DummyOutputStream = Stream.Null;
    private static readonly TextWriter DefaultOutputStream = Console.Out;

    private Benchmarks()
    {
    }

    private static void PrintLine(string value)
    {
        DefaultOutputStream.WriteLine(value);
    }

    public static void Main(string[] args)
    {
        // suppressing output
        Console.SetOut(new StreamWriter(DummyOutputStream) { AutoFlush = true });

        OneLoopUtils.SetUpRiemannSymmetries();
        // TODO: Tensors.AddSymmetry is not yet ported (P_lm symmetry).

        BurnJvm();
        Timer timer = new();
        timer.Start();
        TestMinimalSecondOrderOperator();
        PrintLine($"Minimal second order : {timer.ElapsedTimeInSeconds()} s.");
        timer.Restart();
        TestMinimalFourthOrderOperator();
        PrintLine($"Minimal fourth order : {timer.ElapsedTimeInSeconds()} s.");
        timer.Restart();
        TestVectorField();
        PrintLine($"Vector field : {timer.ElapsedTimeInSeconds()} s.");
        timer.Restart();
        TestGravityGhosts();
        PrintLine($"Gravity ghosts : {timer.ElapsedTimeInSeconds()} s.");
        timer.Restart();
        TestSquaredVectorField();
        PrintLine($"Squared vector field : {timer.ElapsedTimeInSeconds()} s.");
        timer.Restart();
        TestLambdaGaugeGravity();
        PrintLine($"Lambda gauge gravity : {timer.ElapsedTimeInSeconds()} s.");
        timer.Restart();
        TestSpin3Ghosts();
        PrintLine($"Spin 3 ghosts : {timer.ElapsedTimeInSeconds()} s.");
        timer.Restart();
    }

    /// <summary>
    /// Warm up the runtime.
    /// </summary>
    public static void BurnJvm()
    {
        TestVectorField();
        for (int i = 0; i < 10; ++i)
        {
            TestMinimalFourthOrderOperator();
        }

        PrintLine("JVM warmed up.");
    }

    /// <summary>
    /// This method calculates one-loop counterterms of the vector field in the
    /// non-minimal gauge.
    /// </summary>
    public static void TestVectorField()
    {
        Expression iK = TensorFactory.ParseExpression("iK_a^b=d_a^b+c*n_a*n^b");
        Expression K = TensorFactory.ParseExpression("K^{lm}_a^{b}=g^{lm}*d_{a}^{b}-k/2*(g^{lb}*d_a^m+g^{mb}*d_a^l)");
        Expression S = TensorFactory.ParseExpression("S^p^l_m=0");
        Expression W = TensorFactory.ParseExpression("W^{a}_{b}=P^{a}_{b}+(k/2)*R^a_b");
        Expression F = TensorFactory.ParseExpression("F_lmab=R_lmab");

        Expression lambda = TensorFactory.ParseExpression("k=gamma/(1+gamma)");
        Expression gamma = TensorFactory.ParseExpression("c=gamma");
        iK = (Expression)gamma.Transform(lambda.Transform(iK));
        K = (Expression)gamma.Transform(lambda.Transform(K));
        S = (Expression)gamma.Transform(lambda.Transform(S));
        W = (Expression)gamma.Transform(lambda.Transform(W));

        OneLoopInput input = new(2, iK, K, S, W, null!, null!, F);
        OneLoopCounterterms action = OneLoopCounterterms.CalculateOneLoopCounterterms(input);
    }

    /// <summary>
    /// This method calculates one-loop counterterms of the squared vector field
    /// in the non-minimal gauge.
    /// </summary>
    public static void TestSquaredVectorField()
    {
        Expression iK = TensorFactory.ParseExpression("iK_a^b=d_a^b+(2*c+Power[c,2])*n_a*n^b");
        Expression K = TensorFactory.ParseExpression("K^{lmcd}_a^{b}="
            + "d_a^b*1/3*(g^{lm}*g^{cd}+ g^{lc}*g^{md}+ g^{ld}*g^{mc})"
            + "+1/12*(-2*k+Power[k,2])*("
            + "g^{lm}*d_a^c*g^{bd}"
            + "+g^{lm}*d_a^d*g^{bc}"
            + "+g^{lc}*d_a^m*g^{bd}"
            + "+g^{lc}*d_a^d*g^{bm}"
            + "+g^{ld}*d_a^m*g^{bc}"
            + "+g^{ld}*d_a^c*g^{bm}"
            + "+g^{mc}*d_a^l*g^{bd}"
            + "+g^{mc}*d_a^d*g^{bl}"
            + "+g^{md}*d_a^l*g^{bc}"
            + "+g^{md}*d_a^c*g^{bl}"
            + "+g^{cd}*d_a^l*g^{bm}"
            + "+g^{cd}*d_a^m*g^{bl})");
        Expression S = TensorFactory.ParseExpression("S^lmpab=0");
        Expression W = TensorFactory.ParseExpression("W^{lm}_a^b="
            + "2*P_{a}^{b}*g^{lm}-2/3*R^lm*d_a^b"
            + "-k/2*P_a^l*g^mb"
            + "-k/2*P_a^m*g^lb"
            + "-k/2*P^bl*d^m_a"
            + "-k/2*P^bm*d^l_a"
            + "+1/6*(k-2*Power[k,2])*("
            + "R_a^l*g^mb"
            + "+R_a^m*g^lb"
            + "+R^bl*d^m_a"
            + "+R^bm*d^l_a)"
            + "+1/6*(2*k-Power[k,2])*"
            + "(R_a^lbm+R_a^mbl)"
            + "+1/2*(2*k-Power[k,2])*g^lm*R_a^b");
        Expression N = TensorFactory.ParseExpression("N^pab=0");
        Expression M = TensorFactory.ParseExpression("M_a^b = "
            + "P_al*P^lb-1/2*R_lmca*R^lmcb"
            + "+k/2*P_al*R^lb"
            + "+k/2*P_lm*R^l_a^mb"
            + "+1/6*(k-2*Power[k,2])*R_al*R^lb"
            + "+1/12*(4*k+7*Power[k,2])*R_lam^b*R^lm"
            + "+1/4*(2*k-Power[k,2])*R_almc*R^clmb");
        Expression F = TensorFactory.ParseExpression("F_lmab=R_lmab");

        Expression lambda = TensorFactory.ParseExpression("k=gamma/(1+gamma)");
        Expression gamma = TensorFactory.ParseExpression("c=gamma");
        iK = (Expression)gamma.Transform(lambda.Transform(iK));
        K = (Expression)gamma.Transform(lambda.Transform(K));
        S = (Expression)gamma.Transform(lambda.Transform(S));
        W = (Expression)gamma.Transform(lambda.Transform(W));
        M = (Expression)gamma.Transform(lambda.Transform(M));

        OneLoopInput input = new(4, iK, K, S, W, N, M, F);
        OneLoopCounterterms action = OneLoopCounterterms.CalculateOneLoopCounterterms(input);
    }

    /// <summary>
    /// This method calculates ghosts contribution to the one-loop counterterms
    /// of the gravitational field in the non-minimal gauge.
    /// </summary>
    public static void TestGravityGhosts()
    {
        Expression iK = TensorFactory.ParseExpression("iK_a^b=d_a^b+gamma*n_a*n^b");
        Expression K = TensorFactory.ParseExpression("K^{lm}_a^{b}=d_a^b*g^lm-1/2*beta*(d_a^l*g^mb+d_a^m*g^lb)");
        Expression S = TensorFactory.ParseExpression("S^p^l_m=0");
        Expression W = TensorFactory.ParseExpression("W^{a}_{b}=(1+beta/2)*R^a_b");
        Expression F = TensorFactory.ParseExpression("F_lmab=R_lmab");

        Expression beta = TensorFactory.ParseExpression("beta=gamma/(1+gamma)");
        iK = (Expression)beta.Transform(iK);
        K = (Expression)beta.Transform(K);
        S = (Expression)beta.Transform(S);
        W = (Expression)beta.Transform(W);

        OneLoopInput input = new(2, iK, K, S, W, null!, null!, F);
        OneLoopCounterterms action = OneLoopCounterterms.CalculateOneLoopCounterterms(input);
    }

    /// <summary>
    /// This method calculates the main contribution to the one-loop counterterms
    /// of the gravitational field in the non-minimal gauge.
    /// </summary>
    public static void TestLambdaGaugeGravity()
    {
        Expression iK = TensorFactory.ParseExpression("iK_ab^cd = "
            + "(d_a^c*d_b^d+d_b^c*d_a^d)/2-"
            + "la/2*("
            + "d_a^c*n_b*n^d"
            + "+d_a^d*n_b*n^c"
            + "+d_b^c*n_a*n^d"
            + "+d_b^d*n_a*n^c)"
            + "-ga*(g_ab*n^c*n^d+g^cd*n_a*n_b)"
            + "-1/2*g_ab*g^cd"
            + "+2*ga*(ga*la-2*ga+2*la)*n_a*n_b*n^c*n^d");
        Expression K = TensorFactory.ParseExpression("K^lm_ab^cd = "
            + "g^lm*(d_a^c*d_b^d+d_b^c*d_a^d)/2"
            + "-la/(4*(1+la))*("
            + "d_a^c*d_b^l*g^dm"
            + "+d_a^c*d_b^m*g^dl"
            + "+d_a^d*d_b^l*g^cm"
            + "+d_a^d*d_b^m*g^cl"
            + "+d_b^c*d_a^l*g^dm"
            + "+d_b^c*d_a^m*g^dl"
            + "+d_b^d*d_a^l*g^cm"
            + "+d_b^d*d_a^m*g^cl)"
            + "+(la-be)/(2*(1+la))*(g^cd*(d_a^l*d_b^m+d_a^m*d_b^l)+g_ab*(g^cl*g^dm+g^cm*g^dl))"
            + "+g^lm*g_ab*g^cd*(-1+(1+be)**2/(2*(1+la)))");
        K = (Expression)TensorFactory.ParseExpression("be = ga/(1+ga)").Transform(K);
        Expression S = TensorFactory.ParseExpression("S^p_{ab}^{cd}=0");
        Expression W = TensorFactory.ParseExpression("W_{ab}^{cd}=P_ab^cd"
            + "-la/(2*(1+la))*(R_a^c_b^d+R_a^d_b^c)"
            + "+la/(4*(1+la))*("
            + "d_a^c*R_b^d"
            + "+d_a^d*R_b^c"
            + "+d_b^c*R_a^d"
            + "+d_b^d*R_a^c)");
        Expression P = TensorFactory.ParseExpression("P_ab^lm ="
            + "1/4*(d_a^c*d_b^d+d_a^d*d_b^c-g_ab*g^cd)"
            + "*(R_c^l_d^m+R_c^m_d^l-g^lm*R_cd-g_cd*R^lm"
            + "+1/2*(d^l_c*R^m_d+d^m_c*R_d^l+d^l_d*R^m_c+d^m_d*R^l_c)"
            + "-1/2*(d^l_c*d^m_d+d^m_c*d^l_d)*(R-2*LA)+1/2*g_cd*g^lm*R)");
        P = (Expression)ExpandTransformation.Expand(
            P,
            EliminateMetricsTransformation.Instance,
            TensorFactory.ParseExpression("R_{l m}^{l}_{a} = R_{ma}"),
            TensorFactory.ParseExpression("R_{lm}^{a}_{a}=0"),
            TensorFactory.ParseExpression("R_{l}^{l}= R"));
        W = (Expression)P.Transform(W);
        Expression F = TensorFactory.ParseExpression("F_lm^kd_pr = "
            + "R^k_plm*d^d_r+R^d_rlm*d^k_p");

        OneLoopInput input = new(2, iK, K, S, W, null!, null!, F);
        OneLoopCounterterms action = OneLoopCounterterms.CalculateOneLoopCounterterms(input);
    }

    /// <summary>
    /// This method calculates one-loop counterterms of the second order minimal operator.
    /// </summary>
    public static void TestMinimalSecondOrderOperator()
    {
        Expression iK = TensorFactory.ParseExpression("iK_a^b=d_a^b");
        Expression K = TensorFactory.ParseExpression("K^lm_a^b=d_a^b*g^{lm}");
        Expression S = TensorFactory.ParseExpression("S^lab=0");
        Expression W = TensorFactory.ParseExpression("W_a^b=W_a^b");
        Expression F = TensorFactory.ParseExpression("F_lmab=F_lmab");

        OneLoopInput input = new(2, iK, K, S, W, null!, null!, F);
        OneLoopCounterterms action = OneLoopCounterterms.CalculateOneLoopCounterterms(input);
    }

    /// <summary>
    /// This method calculates one-loop counterterms of the second order minimal
    /// operator in Barvinsky and Vilkovisky notation.
    /// </summary>
    public static void TestMinimalSecondOrderOperatorBarvinskyVilkovisky()
    {
        Expression iK = TensorFactory.ParseExpression("iK_a^b=d_a^b");
        Expression K = TensorFactory.ParseExpression("K^lm_a^b=d_a^b*g^{lm}");
        Expression S = TensorFactory.ParseExpression("S^lab=0");
        Expression W = TensorFactory.ParseExpression("W_a^b=W_a^b-1/6*R*d_a^b");
        Expression F = TensorFactory.ParseExpression("F_lmab=F_lmab");

        OneLoopInput input = new(2, iK, K, S, W, null!, null!, F);
        OneLoopCounterterms action = OneLoopCounterterms.CalculateOneLoopCounterterms(input);
    }

    /// <summary>
    /// This method calculates one-loop counterterms of the fourth order minimal operator.
    /// </summary>
    public static void TestMinimalFourthOrderOperator()
    {
        Expression iK = TensorFactory.ParseExpression("iK_a^b=d_a^b");
        Expression K = TensorFactory.ParseExpression("K^{lmcd}_a^{b}="
            + "d_a^b*1/3*(g^{lm}*g^{cd}+ g^{lc}*g^{md}+ g^{ld}*g^{mc})");
        Expression S = TensorFactory.ParseExpression("S^lmpab=0");
        Expression W = TensorFactory.ParseExpression("W^{lm}_a^b=0*W^{lm}_a^b");
        Expression N = TensorFactory.ParseExpression("N^pab=0*N^pab");
        Expression M = TensorFactory.ParseExpression("M_a^b = 0*M_a^b");
        Expression F = TensorFactory.ParseExpression("F_lmab=F_lmab");

        OneLoopInput input = new(4, iK, K, S, W, N, M, F);
        OneLoopCounterterms action = OneLoopCounterterms.CalculateOneLoopCounterterms(input);
    }

    /// <summary>
    /// This method calculates ghosts contribution to the one-loop counterterms
    /// of the theory with spin = 3.
    /// </summary>
    public static void TestSpin3Ghosts()
    {
        Expression iK = TensorFactory.ParseExpression(string.Concat(
            "iK^{ab}_{lm} = P^{ab}_{lm}-1/4*c*g_{lm}*g^{ab}+",
            "(1/4)*b*(n_{l}*n^{a}*d^{b}_{m}+n_{l}*n^{b}*d^{a}_{m}+n_{m}*n^{a}*d^{b}_{l}+n_{m}*n^{b}*d^{a}_{l})+",
            "c*(n_{l}*n_{m}*g^{ab}+n^{a}*n^{b}*g_{lm})",
            "-c*b*n_{l}*n_{m}*n^{a}*n^{b}"));
        Expression K = TensorFactory.ParseExpression(string.Concat(
            "K^{lm}^{ab}_{cd} = g^{lm}*P^{ab}_{cd}+",
            "(1+2*beta)*((1/4)*(d^{l}_{c}*g^{a m}*d^{b}_{d} + d^{l}_{d}*g^{a m}*d^{b}_{c}+d^{l}_{c}*g^{b m}*d^{a}_{d}+ d^{l}_{d}*g^{b m}*d^{a}_{c})+",
            "(1/4)*(d^{m}_{c}*g^{a l}*d^{b}_{d} + d^{m}_{d}*g^{a l}*d^{b}_{c}+d^{m}_{c}*g^{b l}*d^{a}_{d}+ d^{m}_{d}*g^{b l}*d^{a}_{c}) -",
            "(1/4)*(g_{cd}*g^{l a}*g^{m b}+g_{cd}*g^{l b}*g^{m a})-",
            "(1/4)*(g^{ab}*d^{l}_{c}*d^{m}_{d}+g^{ab}*d^{l}_{d}*d^{m}_{c})+(1/8)*g^{lm}*g_{cd}*g^{ab})"));
        Expression P = TensorFactory.ParseExpression(
            "P^{ab}_{lm} = (1/2)*(d^{a}_{l}*d^{b}_{m}+d^{a}_{m}*d^{b}_{l})-(1/4)*g_{lm}*g^{ab}");
        iK = (Expression)P.Transform(iK);
        K = (Expression)P.Transform(K);

        Expression[] constants =
        {
            TensorFactory.ParseExpression("c=(1+2*beta)/(5+6*beta)"),
            TensorFactory.ParseExpression("b=-(1+2*beta)/(1+beta)")
        };
        foreach (Expression constant in constants)
        {
            iK = (Expression)constant.Transform(iK);
            K = (Expression)constant.Transform(K);
        }

        Expression S = TensorFactory.ParseExpression("S^p^{ab}_{lm}=0");
        Expression W = TensorFactory.ParseExpression("W^{ab}_{lm}=0");
        Expression F = TensorFactory.ParseExpression("F_lmabcd=0");

        Expression[] ds = OneLoopUtils.AntiDeSitterBackground();
        ITransformation[] tr = new ITransformation[ds.Length + 1];
        for (int i = 0; i < ds.Length; i++)
        {
            tr[i] = ds[i];
        }

        tr[^1] = FactorTransformation.Instance;
        OneLoopInput input = new(2, iK, K, S, W, null!, null!, F, tr);
        OneLoopCounterterms action = OneLoopCounterterms.CalculateOneLoopCounterterms(input);
    }

    /// <summary>
    /// This method calculates the main contribution to the one-loop counterterms
    /// of the gravitational field in general the non-minimal gauge.
    /// </summary>
    public static void TestNonMinimalGaugeGravity()
    {
        Expression iK = TensorFactory.ParseExpression("iK_ab^cd = "
            + "(d_a^c*d_b^d+d_b^c*d_a^d)/2-"
            + "la/2*("
            + "d_a^c*n_b*n^d"
            + "+d_a^d*n_b*n^c"
            + "+d_b^c*n_a*n^d"
            + "+d_b^d*n_a*n^c)"
            + "-ga*(g_ab*n^c*n^d+g^cd*n_a*n_b)"
            + "-1/2*g_ab*g^cd"
            + "+2*ga*(ga*la-2*ga+2*la)*n_a*n_b*n^c*n^d");
        Expression K = TensorFactory.ParseExpression("K^lm_ab^cd = "
            + "g^lm*(d_a^c*d_b^d+d_b^c*d_a^d)/2"
            + "-la/(4*(1+la))*("
            + "d_a^c*d_b^l*g^dm"
            + "+d_a^c*d_b^m*g^dl"
            + "+d_a^d*d_b^l*g^cm"
            + "+d_a^d*d_b^m*g^cl"
            + "+d_b^c*d_a^l*g^dm"
            + "+d_b^c*d_a^m*g^dl"
            + "+d_b^d*d_a^l*g^cm"
            + "+d_b^d*d_a^m*g^cl)"
            + "+(la-be)/(2*(1+la))*(g^cd*(d_a^l*d_b^m+d_a^m*d_b^l)+g_ab*(g^cl*g^dm+g^cm*g^dl))"
            + "+g^lm*g_ab*g^cd*(-1+(1+be)**2/(2*(1+la)))");
        K = (Expression)TensorFactory.ParseExpression("be = ga/(1+ga)").Transform(K);
        Expression S = TensorFactory.ParseExpression("S^p_{ab}^{cd}=0");
        Expression W = TensorFactory.ParseExpression("W_{ab}^{cd}=P_ab^cd"
            + "-la/(2*(1+la))*(R_a^c_b^d+R_a^d_b^c)"
            + "+la/(4*(1+la))*("
            + "d_a^c*R_b^d"
            + "+d_a^d*R_b^c"
            + "+d_b^c*R_a^d"
            + "+d_b^d*R_a^c)");
        Expression P = TensorFactory.ParseExpression("P_ab^lm ="
            + "1/4*(d_a^c*d_b^d+d_a^d*d_b^c-g_ab*g^cd)"
            + "*(R_c^l_d^m+R_c^m_d^l-g^lm*R_cd-g_cd*R^lm"
            + "+1/2*(d^l_c*R^m_d+d^m_c*R_d^l+d^l_d*R^m_c+d^m_d*R^l_c)"
            + "-1/2*(d^l_c*d^m_d+d^m_c*d^l_d)*(R-2*LA)+1/2*g_cd*g^lm*R)");
        P = (Expression)ExpandTransformation.Expand(
            P,
            EliminateMetricsTransformation.Instance,
            TensorFactory.ParseExpression("R_{l m}^{l}_{a} = R_{ma}"),
            TensorFactory.ParseExpression("R_{lm}^{a}_{a}=0"),
            TensorFactory.ParseExpression("R_{l}^{l}= R"));
        W = (Expression)P.Transform(W);
        Expression F = TensorFactory.ParseExpression("F_lm^kd_pr = "
            + "R^k_plm*d^d_r+R^d_rlm*d^k_p");

        OneLoopInput input = new(2, iK, K, S, W, null!, null!, F);
        OneLoopCounterterms action = OneLoopCounterterms.CalculateOneLoopCounterterms(input);
    }

    public sealed class Timer
    {
        private readonly Stopwatch _stopwatch = new();

        public void Start()
        {
            _stopwatch.Restart();
        }

        public long ElapsedTime()
        {
            return _stopwatch.ElapsedMilliseconds;
        }

        public long ElapsedTimeInSeconds()
        {
            return _stopwatch.ElapsedMilliseconds / 1000;
        }

        public void Restart()
        {
            Start();
        }
    }
}
