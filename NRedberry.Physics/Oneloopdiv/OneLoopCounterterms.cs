using NRedberry;
using NRedberry.Core.Utils;
using NRedberry.Indices;
using NRedberry.Parsers;
using NRedberry.Parsers.Preprocessor;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Port of cc.redberry.physics.oneloopdiv.OneLoopCounterterms.
/// </summary>
public sealed class OneLoopCounterterms
{
    private static readonly string s_flat = string.Concat(
        "Flat=",
        "(1/4)*HATS*HATS*HATS*HATS-HATW*HATS*HATS+(1/2)*HATW*HATW+HATS*HATN-HATM+(L-2)*NABLAS_l*HATW^l",
        "-L*NABLAS_l*HATW*HATK^l+(1/3)*((L-1)*NABLAS_l^l*HATS*HATS-L*NABLAS_l*HATK^l*HATS*HATS",
        "-(L-1)*NABLAS_l*HATS*HATS^l+L*NABLAS_l*HATS*HATS*HATK^l)-(1/2)*NABLAS_l*NABLAS_m*DELTA^{lm}",
        "-(1/4)*(L-1)*(L-2)*NABLAS_l*NABLAS_m^{lm}+(1/2)*L*(L-1)*(1/2)*(NABLAS_l*NABLAS_{m }^{m}",
        "+NABLAS_{m }*NABLAS_{l }^{m})*HATK^l");

    private static readonly string s_wr = string.Concat(
        "WR=",
        "-(1/2)*Power[L,2]*HATW*HATF_{lm}*Kn^l*HATK^m",
        "+(1/3)*L*HATW*HATK^a*DELTA^{lm}*n_q*R^q_{lam}",
        "+(1/3)*Power[L,2]*(L-1)*HATW*HATK^{lm}*HATK^a*n_q*R^q_{lam}",
        "-(1/6)*(L-2)*(L-3)*HATW^{lm}*R_{lm}");

    private static readonly string s_sr = string.Concat(
        "SR=-(1/6)*Power[L,2]*(L-1)*HATS*NABLAF_{lam}*Kn^{lm}*HATK^a",
        "+(2/3)*L*HATS*NABLAF_{lma}*Kn^a*DELTA^{lm}",
        "-(1/12)*(L-1)*(L-2)*(L-3)*HATS^{alm}*NABLAR_{alm}",
        "-(1/12)*Power[L,2]*(L-1)*(L-2)*HATS*HATK^{lma}*HATK^b*n_q*NABLAR_a^q_{lbm}",
        "+L*(L-1)*HATS*HATK^{lm}*DELTA^{ab}*n_q*((5/12)*NABLAR_a^q_{mbl}",
        "-(1/12)*NABLAR_{l}^q_{amb})",
        "-(1/2)*L*HATS*HATK^b*DELTA^{lma}*n_q*NABLAR_{a}^{q}_{lbm}");

    private static readonly string s_ssr = string.Concat(
        "SSR=-(1/2)*L*(L-1)*HATS*HATS^l*HATF_{lm}*HATK^{m}+(1/2)*Power[L,2]*HATS*HATS*HATF_{lm}*Kn^{l}*HATK^m",
        "+(1/12)*(L-1)*(L-2)*HATS*HATS^{lm}*R_{lm}+(1/3)*L*(L-1)*HATS*HATS^l*HATK^m*R_{lm}",
        "+(1/6)*HATS*HATS*DELTA^{lm}*R_{lm}-(1/6)*L*(L-1)*(L-2)*HATS*HATS^{lm}*HATK^a*n_q*R^q_{lam}",
        "+(1/3)*(L-1)*HATS*HATS^a*DELTA^{lm}*n_q*R^q_{lam}",
        "-(1/3)*Power[L,2]*(L-1)*HATS*HATS*HATK^{lm}*HATK^a*n_q*R^q_{lam}",
        "-(1/3)*L*HATS*HATS*HATK^a*DELTA^{lm}*n_q*R^q_{lam}");

    private static readonly string s_ff = string.Concat(
        "FF=",
        "-(1/24)*L*L*(L-1)*(L-1)*HATK^{lm}*F_{la}*HATK^{ab}*F_{mb}",
        "+(1/24)*L*L*HATK^l*F_{bm}*DELTA^{ab}*HATK^m*F_{al}",
        "-(5/24)*L*L*HATK^l*F_{bl}*DELTA^{ab}*HATK^m*F_{am}",
        "-(1/48)*L*L*(L-1)*HATK^l*F_{bm}*DELTA^m*HATK^{ab}*F_{al}",
        "-(1/48)*L*L*(L-1)*HATK^l*F_{bl}*DELTA^m*HATK^{ab}*F_{am}");

    private static readonly string s_fr = string.Concat(
        "FR=",
        "(1/40)*Power[L,2]*(L-1)*(L-2)*DELTA^l*HATK^m*HATK^{abc}*F_{la}*n_q*R^q_{cbm}",
        "-Power[L,2]*(L-1)*(L-2)*DELTA^m*HATK^{abc}*HATK^l*n_q*((1/60)*R^q_{bcl}*F_{am}",
        "+(1/12)*R^q_{bcm}*F_{al})",
        "+Power[L,2]*Power[(L-1),2]*DELTA^a*HATK^{bc}*HATK^{lm}*n_q*((1/60)*R^q_{blc}*F_{am}",
        "+(1/20)*R^q_{alc}*F_{mb}+(1/15)*R^q_{cla}*F_{mb}",
        "+(1/60)*R^q_{lmc}*F_{ab})+Power[L,2]*(L-1)*DELTA^{ab}*HATK^{cd}*HATK^{l}",
        "*n_q*((4/15)*R^q_{dbc}*F_{al}-(1/30)*R^q_{bda}*F_{cl}",
        "-(1/15)*R^q_{acl}*F_{bd}-(1/30)*R^q_{cal}*F_{bd})",
        "+Power[L,2]*(L-1)*DELTA^{ab}*HATK^c*HATK^{lm}*n_q*((7/60)*R^q_{abl}*F_{cm}",
        "-(11/60)*R^q_{blc}*F_{am}+(1/5)*R^q_{lac}*F_{bm}",
        "+(1/60)*R^q_{lam}*F_{cb})",
        "+Power[L,2]*DELTA^{lab}*HATK^c*HATK^m*n_q",
        "*((7/20)*R^q_{acb}*F_{ml}+(1/10)*R^q_{abm}*F_{cl})");

    private static readonly string s_rr = string.Concat(
        "RR=",
        "(1/10)*Power[L,2]*HATK^d*DELTA^{lmab}*HATK^c*n_q*n_p*R^q_{abc}*R^p_{lmd}",
        "+Power[L,2]*Power[(L-1),2]*(L-2)*HATK^{bcd}*DELTA^a*HATK^{lm}*n_q*n_p*",
        "((2/45)*R^p_{adm}*R^q_{blc}-(1/120)*R^p_{dam}*R^q_{blc})",
        "+Power[L,2]*(L-1)*HATK^d*DELTA^{abc}*HATK^{lm}*n_q*n_p*",
        "((-1/10)*R^p_{lcm}*R^q_{adb}+(1/15)*R^p_{dam}*R^q_{blc}+(1/60)*R^p_{bdm}*R^q_{cla})",
        "+Power[L,2]*Power[(L-1),2]*HATK^{cd}*DELTA^{ab}*HATK^{lm}*n_q*n_p*",
        "(-(1/20)*R^p_{lbm}*R^q_{dac}+(1/180)*R^p_{amb}*R^q_{cdl}-(7/360)*R^p_{lcm}*R^q_{adb}-(1/240)*R^p_{dbm}*R^q_{cal}-(1/120)*R^p_{bcm}*R^q_{adl}-(1/30)*R^p_{dbm}*R^q_{acl})",
        "+Power[L,2]*(L-1)*(L-2)*HATK^d*DELTA^{lm}*HATK^{abc}*n_q*n_p*",
        "((-1/30)*R^p_{cmb}*R^q_{adl}-(1/180)*R^p_{lcm}*R^q_{abd}+(1/180)*R^p_{lcd}*R^q_{abm})",
        "+Power[L,2]*Power[(L-1),2]*(L-2)*HATK^{lm}*DELTA^{d}*HATK^{abc}*n_q*n_p*",
        "((1/45)*R^p_{lcm}*R^q_{abd}-(1/80)*R^p_{bmc}*R^q_{lad}+(1/90)*R^p_{bmc}*R^q_{dal})",
        "+Power[L,2]*(L-1)*HATK^{lm}*DELTA^{abc}*HATK^d*n_q*n_p*",
        "((7/120)*R^p_{bcm}*R^q_{lad}-(3/40)*R^p_{bcd}*R^q_{lam}+(1/120)*R^p_{dcm}*R^q_{abl})",
        "+Power[L,2]*(L-1)*(L-2)*HATK^{abc}*DELTA^{lm}*HATK^d*n_q*n_p*",
        "(-(1/24)*R^p_{lcm}*R^q_{abd}-(1/180)*R^p_{mcd}*R^q_{abl}-(1/360)*R^p_{dcm}*R^q_{abl})",
        "-(1/120)*Power[L,2]*(L-1)*(L-2)*(L-3)*HATK^{lmab}*DELTA^{d}*HATK^c*n_q*n_p*R^p_{abc}*R^q_{lmd}",
        "-(1/80)*Power[L,2]*Power[(L-1),2]*(L-2)*(L-3)*HATK^{abcd}*HATK^{lm}*n_q*n_p*R^p_{bcl}*R^q_{adm}",
        "+Power[L,2]*HATK^l*DELTA^{abc}*HATK^m*n_p*(-(1/8)*R_{bc}*R^p_{mal}+(3/20)*R_{bc}*R^p_{lam}+(3/40)*R_{al}*R^p_{bcm}+(1/40)*R^q_{bcl}*R^p_{maq}-(3/20)*R^q_{abl}*R^p_{cmq}+(1/10)*R^q_{abm}*R^p_{clq})",
        "+Power[L,2]*(L-1)*HATK^c*DELTA^{ab}*HATK^{lm}*n_p*",
        "((1/20)*R_{am}*R^p_{cbl}+(1/20)*R_{ac}*R^p_{lbm}+(1/10)*R_{ab}*R^p_{lcm}+(1/20)*R^q_{amc}*R^p_{qbl}-(1/60)*R^q_{lam}*R^p_{bqc}+(1/10)*R^q_{abc}*R^p_{lqm}-(1/12)*R^q_{abm}*R^p_{lqc})",
        "+Power[L,2]*Power[(L-1),2]*HATK^{ab}*DELTA^{c}*HATK^{lm}*n_p*",
        "((1/60)*R_{al}*R^p_{bmc}-(1/20)*R_{al}*R^p_{cmb}+(1/120)*R_{ab}*R^p_{lmc}+(3/40)*R_{ac}*R^p_{mbl}+(1/20)*R^q_{cla}*R^p_{mqb}+(1/120)*R^q_{alc}*R^p_{bmq}-(1/40)*R^q_{alc}*R^p_{qmb}+(1/40)*R^q_{alb}*R^p_{qmc}-(1/20)*R^q_{alb}*R^p_{cmq}-(1/40)*R^q_{lbm}*R^p_{cqa})",
        "+Power[L,2]*(L-1)*HATK^{ab}*DELTA^{lm}*HATK^{c}*n_p*",
        "((1/20)*R^q_{lmb}*R^p_{cqa}-(7/60)*R^q_{bla}*R^p_{cmq}+(1/20)*R^q_{bla}*R^p_{qmc}+(1/10)*R^q_{lbc}*R^p_{maq}+(1/60)*R^q_{blc}*R^p_{amq}+(7/120)*R_{ab}*R^p_{mcl}+(11/60)*R_{bl}*R^p_{mac})",
        "+Power[L,2]*(L-1)*(L-2)*HATK^{abc}*DELTA^{l}*HATK^{m}*n_p*",
        "((7/240)*R_{ab}*R^p_{clm}+(7/240)*R_{am}*R^p_{bcl}-(1/60)*R_{al}*R^p_{bcm}-(1/24)*R^q_{abm}*R^p_{qcl}+(1/15)*R^q_{abm}*R^p_{lcq}+(1/40)*R^q_{abl}*R^p_{qcm}+(1/40)*R_{bc}*R^p_{mla}+(1/48)*R^q_{bcl}*R^p_{maq})",
        "+Power[L,2]*Power[(L-1),2]*(L-2)*HATK^{abc}*HATK^{lm}*n_p*",
        "((-7/240)*R_{al}*R^p_{bcm}+(1/240)*R_{bc}*R^p_{lam}-(1/40)*R^q_{alb}*R^p_{mcq})",
        "+L*(L-1)*(L-2)*(L-3)*HATK^{lmab}*",
        "((1/180)*R_{lm}*R_{ab}+(7/720)*R^q_{abp}*R^p_{lmq})");

    private static readonly string s_delta1 = "DELTA^l=-L*HATK^l";

    private static readonly string s_delta2 =
        "DELTA^{lm}=-(1/2)*L*(L-1)*HATK^{lm}+Power[L,2]*(1/2)*(HATK^{l }*HATK^{m }+HATK^{m }*HATK^{l })";

    private static readonly string s_delta3 = string.Concat(
        "DELTA^{lma}=",
        "-(1/6)*L*(L-1)*(L-2)*HATK^{lma}",
        "+(1/2)*Power[L,2]*(L-1)*(1/3)*(",
        "HATK^{l m }*HATK^{a }+",
        "HATK^{a m }*HATK^{l }+",
        "HATK^{l a }*HATK^{m })",
        "+1/2*Power[L,2]*(L-1)*(1/3)*(",
        "HATK^{a }*HATK^{l m }+",
        "HATK^{l }*HATK^{a m }+",
        "HATK^{m }*HATK^{a l })",
        "-Power[L,3]*(1/6)*(",
        "HATK^{l }*HATK^{m }*HATK^{a }+",
        "HATK^{l }*HATK^{a }*HATK^{m }+",
        "HATK^{m }*HATK^{a }*HATK^{l }+",
        "HATK^{m }*HATK^{l }*HATK^{a }+",
        "HATK^{a }*HATK^{l }*HATK^{m }+",
        "HATK^{a }*HATK^{m }*HATK^{l })");

    private static readonly string s_delta4 = string.Concat(
        "DELTA^{lmab}=",
        "-(1/24)*L*(L-1)*(L-2)*(L-3)*HATK^{lmab}",
        "+(1/6)*Power[L,2]*(L-1)*(L-2)*(1/4)*(",
        "HATK^{l m a }*HATK^{b }+",
        "HATK^{l m b }*HATK^{a }+",
        "HATK^{b l a }*HATK^{m }+",
        "HATK^{m b a }*HATK^{l })",
        "+(1/6)*Power[L,2]*(L-1)*(L-2)*(1/4)*(",
        "HATK^{b }*HATK^{l m a }+",
        "HATK^{a }*HATK^{l m b }+",
        "HATK^{l }*HATK^{b m a }+",
        "HATK^{m }*HATK^{b l a })",
        "+(1/4)*Power[L,2]*Power[(L-1),2]*(1/6)*(",
        "HATK^{lm}*HATK^{ab}+",
        "HATK^{lb}*HATK^{am}+",
        "HATK^{la}*HATK^{mb}+",
        "HATK^{am}*HATK^{lb}+",
        "HATK^{bm}*HATK^{al}+",
        "HATK^{ab}*HATK^{lm})",
        "-(1/2)*Power[L,3]*(L-1)*(1/12)*(",
        "HATK^{lm}*HATK^a*HATK^b+",
        "HATK^{lm}*HATK^b*HATK^a+",
        "HATK^{lb}*HATK^a*HATK^m+",
        "HATK^{lb}*HATK^m*HATK^a+",
        "HATK^{la}*HATK^m*HATK^b+",
        "HATK^{la}*HATK^b*HATK^m+",
        "HATK^{ma}*HATK^l*HATK^b+",
        "HATK^{ma}*HATK^b*HATK^l+",
        "HATK^{mb}*HATK^a*HATK^l+",
        "HATK^{mb}*HATK^l*HATK^a+",
        "HATK^{ab}*HATK^l*HATK^m+",
        "HATK^{ab}*HATK^m*HATK^l)",
        "-(1/2)*Power[L,3]*(L-1)*(1/12)*(",
        "HATK^a*HATK^{lm}*HATK^b+",
        "HATK^b*HATK^{lm}*HATK^a+",
        "HATK^a*HATK^{lb}*HATK^m+",
        "HATK^m*HATK^{lb}*HATK^a+",
        "HATK^m*HATK^{la}*HATK^b+",
        "HATK^b*HATK^{la}*HATK^m+",
        "HATK^l*HATK^{ma}*HATK^b+",
        "HATK^b*HATK^{ma}*HATK^l+",
        "HATK^a*HATK^{mb}*HATK^l+",
        "HATK^l*HATK^{mb}*HATK^a+",
        "HATK^l*HATK^{ab}*HATK^m+",
        "HATK^m*HATK^{ab}*HATK^l)",
        "-(1/2)*Power[L,3]*(L-1)*(1/12)*(",
        "HATK^a*HATK^b*HATK^{lm}+",
        "HATK^b*HATK^a*HATK^{lm}+",
        "HATK^a*HATK^m*HATK^{lb}+",
        "HATK^m*HATK^a*HATK^{lb}+",
        "HATK^m*HATK^b*HATK^{la}+",
        "HATK^b*HATK^m*HATK^{la}+",
        "HATK^l*HATK^b*HATK^{ma}+",
        "HATK^b*HATK^l*HATK^{ma}+",
        "HATK^a*HATK^l*HATK^{mb}+",
        "HATK^l*HATK^a*HATK^{mb}+",
        "HATK^l*HATK^m*HATK^{ab}+",
        "HATK^m*HATK^l*HATK^{ab})",
        "+(1/24)*Power[L,4]*(",
        "HATK^{l}*HATK^{m}*HATK^{a}*HATK^{b}+",
        "HATK^{m}*HATK^{l}*HATK^{a}*HATK^{b}+",
        "HATK^{b}*HATK^{m}*HATK^{a}*HATK^{l}+",
        "HATK^{m}*HATK^{b}*HATK^{a}*HATK^{l}+",
        "HATK^{b}*HATK^{l}*HATK^{a}*HATK^{m}+",
        "HATK^{l}*HATK^{b}*HATK^{a}*HATK^{m}+",
        "HATK^{l}*HATK^{m}*HATK^{b}*HATK^{a}+",
        "HATK^{m}*HATK^{l}*HATK^{b}*HATK^{a}+",
        "HATK^{a}*HATK^{m}*HATK^{b}*HATK^{l}+",
        "HATK^{m}*HATK^{a}*HATK^{b}*HATK^{l}+",
        "HATK^{a}*HATK^{l}*HATK^{b}*HATK^{m}+",
        "HATK^{l}*HATK^{a}*HATK^{b}*HATK^{m}+",
        "HATK^{b}*HATK^{m}*HATK^{l}*HATK^{a}+",
        "HATK^{m}*HATK^{b}*HATK^{l}*HATK^{a}+",
        "HATK^{a}*HATK^{m}*HATK^{l}*HATK^{b}+",
        "HATK^{m}*HATK^{a}*HATK^{l}*HATK^{b}+",
        "HATK^{a}*HATK^{b}*HATK^{l}*HATK^{m}+",
        "HATK^{b}*HATK^{a}*HATK^{l}*HATK^{m}+",
        "HATK^{b}*HATK^{l}*HATK^{m}*HATK^{a}+",
        "HATK^{l}*HATK^{b}*HATK^{m}*HATK^{a}+",
        "HATK^{a}*HATK^{l}*HATK^{m}*HATK^{b}+",
        "HATK^{l}*HATK^{a}*HATK^{m}*HATK^{b}+",
        "HATK^{a}*HATK^{b}*HATK^{m}*HATK^{l}+",
        "HATK^{b}*HATK^{a}*HATK^{m}*HATK^{l})");

    private static readonly string s_action = "counterterms = Flat + WR + SR + SSR + FF + FR + RR";

    private OneLoopCounterterms(
        Expression flat,
        Expression wr,
        Expression sr,
        Expression ssr,
        Expression ff,
        Expression fr,
        Expression rr,
        Expression delta1,
        Expression delta2,
        Expression delta3,
        Expression delta4,
        Expression action)
    {
        Flat = flat;
        Wr = wr;
        Sr = sr;
        Ssr = ssr;
        Ff = ff;
        Fr = fr;
        Rr = rr;
        Delta1 = delta1;
        Delta2 = delta2;
        Delta3 = delta3;
        Delta4 = delta4;
        Counterterms = action;
    }

    public Expression Flat { get; }

    public Expression Wr { get; }

    public Expression Sr { get; }

    public Expression Ssr { get; }

    public Expression Ff { get; }

    public Expression Fr { get; }

    public Expression Rr { get; }

    public Expression Counterterms { get; }

    public Expression Delta1 { get; }

    public Expression Delta2 { get; }

    public Expression Delta3 { get; }

    public Expression Delta4 { get; }

    public static OneLoopCounterterms CalculateOneLoopCounterterms(OneLoopInput input)
    {
        string[] matrices =
        [
            "iK",
            "HATK",
            "HATW",
            "HATS",
            "NABLAS",
            "HATN",
            "HATF",
            "NABLAF",
            "HATM",
            "DELTA",
            "Flat",
            "FF",
            "WR",
            "SR",
            "SSR",
            "FR",
            "RR",
            "Kn"
        ];

        StructureOfIndices fTypeStructure = StructureOfIndices.Create(IndexType.LatinLower, 2);
        IIndicator<ParseTokenSimpleTensor> matricesIndicator = new MatricesIndicator(matrices, fTypeStructure);

        int matrixIndicesCount = input.GetMatrixIndicesCount();
        int operatorOrder = input.GetOperatorOrder();

        int[] upper = new int[matrixIndicesCount / 2];
        int[] lower = (int[])upper.Clone();
        for (int i = 0; i < matrixIndicesCount / 2; ++i)
        {
            upper[i] = IndicesUtils.CreateIndex(130 + i, IndexType.LatinLower, true);
            lower[i] = IndicesUtils.CreateIndex(130 + i + matrixIndicesCount / 2, IndexType.LatinLower, false);
        }

        IndicesInsertion termIndicesInsertion = new(
            IndicesFactory.CreateSimple(null, upper),
            IndicesFactory.CreateSimple(null, IndicesUtils.GetIndicesNames(upper)),
            matricesIndicator);

        Expression flat = (Expression)TensorFactory.Parse(s_flat, termIndicesInsertion);
        Expression wr = (Expression)TensorFactory.Parse(s_wr, termIndicesInsertion);
        Expression sr = (Expression)TensorFactory.Parse(s_sr, termIndicesInsertion);
        Expression ssr = (Expression)TensorFactory.Parse(s_ssr, termIndicesInsertion);
        Expression ff = (Expression)TensorFactory.Parse(s_ff, termIndicesInsertion);
        Expression fr = (Expression)TensorFactory.Parse(s_fr, termIndicesInsertion);
        Expression rr = (Expression)TensorFactory.Parse(s_rr, termIndicesInsertion);
        Expression action = (Expression)TensorFactory.Parse(s_action, termIndicesInsertion);
        Expression[] terms = [flat, wr, sr, ssr, ff, fr, rr];

        IndicesInsertion deltaIndicesInsertion = new(
            IndicesFactory.CreateSimple(null, upper),
            IndicesFactory.CreateSimple(null, lower),
            matricesIndicator);

        Expression delta1 = (Expression)TensorFactory.Parse(s_delta1, deltaIndicesInsertion);
        Expression delta2 = (Expression)TensorFactory.Parse(s_delta2, deltaIndicesInsertion);
        Expression delta3 = (Expression)TensorFactory.Parse(s_delta3, deltaIndicesInsertion);
        Expression delta4 = (Expression)TensorFactory.Parse(s_delta4, deltaIndicesInsertion);
        Expression[] deltaExpressions = [delta1, delta2, delta3, delta4];

        Expression fSubstitution = input.GetF();
        foreach (ITransformation background in input.GetRiemannBackground())
        {
            fSubstitution = (Expression)background.Transform(fSubstitution);
        }

        Expression[] riemansSubstitutions =
        [
            fSubstitution,
            TensorFactory.ParseExpression("R_{l m}^{l}_{a} = R_{ma}"),
            TensorFactory.ParseExpression("R_{lm}^{a}_{a}=0"),
            TensorFactory.ParseExpression("F_{l}^{l}^{a}_{b}=0"),
            TensorFactory.ParseExpression("R_{lmab}*R^{lamb}=(1/2)*R_{lmab}*R^{lmab}"),
            TensorFactory.ParseExpression("R_{lmab}*R^{lmab}=4*R_{lm}*R^{lm}-R*R"),
            TensorFactory.ParseExpression("R_{l}^{l}= R")
        ];

        Expression kronecker = (Expression)TensorFactory.Parse("d_{l}^{l}=4");
        ITransformation n2 = new SqrSubs(TensorFactory.ParseSimple("n_l"));
        ITransformation n2Transformer = new Transformer(TraverseState.Leaving, new[] { n2 });
        ITransformation[] common = [EliminateMetricsTransformation.Instance, n2Transformer, kronecker];
        ITransformation[] all = new ITransformation[common.Length + riemansSubstitutions.Length];
        Array.Copy(common, 0, all, 0, common.Length);
        Array.Copy(riemansSubstitutions, 0, all, common.Length, riemansSubstitutions.Length);
        Tensor temp;

        for (int i = 0; i < 2; ++i)
        {
            temp = deltaExpressions[i];
            temp = input.GetL().Transform(temp);

            foreach (Expression hatK in input.GetHatQuantities(0))
            {
                temp = hatK.Transform(temp);
            }

            temp = ExpandTransformation.Expand(temp, common);
            foreach (ITransformation tr in common)
            {
                temp = tr.Transform(temp);
            }

            deltaExpressions[i] = (Expression)temp;
        }

        Tensor[] combinations =
        [
            TensorFactory.Parse("HATK^{lma}", deltaIndicesInsertion),
            TensorFactory.Parse("HATK^{lm}*HATK^{a}", deltaIndicesInsertion),
            TensorFactory.Parse("HATK^{a}*HATK^{lm}", deltaIndicesInsertion),
            TensorFactory.Parse("HATK^{l}*HATK^{m}*HATK^{a}", deltaIndicesInsertion)
        ];
        Expression[] calculatedCombinations = new Expression[combinations.Length];
        for (int i = 0; i < combinations.Length; ++i)
        {
            temp = combinations[i];
            foreach (Expression hatK in input.GetHatQuantities(0))
            {
                temp = hatK.Transform(temp);
            }

            temp = ExpandTransformation.Expand(temp, common);
            foreach (ITransformation tr in common)
            {
                temp = tr.Transform(temp);
            }

            calculatedCombinations[i] = TensorFactory.Expression(combinations[i], temp);
        }

        temp = delta3;
        temp = input.GetL().Transform(temp);
        foreach (Expression t in calculatedCombinations)
        {
            temp = new NaiveSubstitution(t[0], t[1]).Transform(temp);
        }

        temp = ExpandTransformation.Expand(temp, common);
        foreach (ITransformation tr in common)
        {
            temp = tr.Transform(temp);
        }

        deltaExpressions[2] = (Expression)temp;

        combinations =
        [
            TensorFactory.Parse("HATK^{lmab}", deltaIndicesInsertion),
            TensorFactory.Parse("HATK^{lma}*HATK^{b}", deltaIndicesInsertion),
            TensorFactory.Parse("HATK^{b}*HATK^{lma }", deltaIndicesInsertion),
            TensorFactory.Parse("HATK^{ab}*HATK^{lm}", deltaIndicesInsertion),
            TensorFactory.Parse("HATK^{l}*HATK^{m}*HATK^{ab}", deltaIndicesInsertion),
            TensorFactory.Parse("HATK^{l}*HATK^{ab}*HATK^{m}", deltaIndicesInsertion),
            TensorFactory.Parse("HATK^{ab}*HATK^{l}*HATK^{m}", deltaIndicesInsertion),
            TensorFactory.Parse("HATK^{b}*HATK^{a}*HATK^{l}*HATK^{m}", deltaIndicesInsertion)
        ];
        calculatedCombinations = new Expression[combinations.Length];
        for (int i = 0; i < combinations.Length; ++i)
        {
            temp = combinations[i];
            foreach (Expression hatK in input.GetHatQuantities(0))
            {
                temp = hatK.Transform(temp);
            }

            temp = ExpandTransformation.Expand(temp, common);
            foreach (ITransformation tr in common)
            {
                temp = tr.Transform(temp);
            }

            calculatedCombinations[i] = TensorFactory.Expression(combinations[i], temp);
        }

        temp = delta4;
        temp = input.GetL().Transform(temp);
        foreach (Expression t in calculatedCombinations)
        {
            temp = new NaiveSubstitution(t[0], t[1]).Transform(temp);
        }

        temp = ExpandTransformation.Expand(temp, common);
        foreach (ITransformation tr in common)
        {
            temp = tr.Transform(temp);
        }

        deltaExpressions[3] = (Expression)temp;

        for (int i = 0; i < terms.Length; ++i)
        {
            temp = terms[i];
            temp = input.GetL().Transform(temp);

            temp = input.GetF().Transform(temp);
            temp = input.GetHatF().Transform(temp);
            foreach (ITransformation riemannBackground in input.GetRiemannBackground())
            {
                temp = riemannBackground.Transform(temp);
            }

            foreach (ITransformation tr in all)
            {
                temp = tr.Transform(temp);
            }

            foreach (Expression nabla in input.GetNablaS())
            {
                temp = nabla.Transform(temp);
            }

            temp = input.GetF().Transform(temp);
            temp = input.GetHatF().Transform(temp);

            foreach (Expression kn in input.GetKnQuantities())
            {
                temp = kn.Transform(temp);
            }

            foreach (Expression[] hatQuantities in input.GetHatQuantities())
            {
                foreach (Expression hatQ in hatQuantities)
                {
                    temp = hatQ.Transform(temp);
                }
            }

            foreach (Expression delta in deltaExpressions)
            {
                temp = delta.Transform(temp);
            }

            temp = ExpandTransformation.Expand(temp, all);
            foreach (ITransformation tr in all)
            {
                temp = tr.Transform(temp);
            }

            temp = new Averaging(TensorFactory.ParseSimple("n_l")).Transform(temp);

            temp = ExpandTransformation.Expand(temp, all);
            foreach (ITransformation tr in all)
            {
                temp = tr.Transform(temp);
            }

            temp = ExpandTransformation.Expand(temp, all);
            terms[i] = (Expression)temp;
        }

        foreach (Expression term in terms)
        {
            action = (Expression)term.Transform(action);
        }

        return new OneLoopCounterterms(
            flat,
            wr,
            sr,
            ssr,
            ff,
            fr,
            rr,
            deltaExpressions[0],
            deltaExpressions[1],
            deltaExpressions[2],
            deltaExpressions[3],
            action);
    }
}

internal sealed class MatricesIndicator : IIndicator<ParseTokenSimpleTensor>
{
    private readonly HashSet<string> _matrices;
    private readonly StructureOfIndices _fTypeStructure;

    public MatricesIndicator(IEnumerable<string> matrices, StructureOfIndices fTypeStructure)
    {
        _matrices = new HashSet<string>(matrices);
        _fTypeStructure = fTypeStructure;
    }

    public bool Is(ParseTokenSimpleTensor @object)
    {
        if (_matrices.Contains(@object.Name))
        {
            return true;
        }

        if (string.Equals(@object.Name, "F", StringComparison.Ordinal)
            && @object.Indices.StructureOfIndices.Equals(_fTypeStructure))
        {
            return true;
        }

        return false;
    }
}
