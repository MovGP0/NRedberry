# Java Algebra System (JAS)

A powerful graphing computer algebra system that is capable of performing algebraic simplifications, manipulations, and some calculus.

[GitHub: Java Algebra System (JAS)](https://github.com/kredel/java-algebra-system/)

## Domains

- **Arith:** integer, rational, modular, and combinatorial arithmetic primitives (e.g., `BigInteger`, `ModLongRing`, `ModularNotInvertibleException`).
- **GB:** contains [Gröbner basis](https://en.wikipedia.org/wiki/Gr%C3%B6bner_basis) engines and reductions
- **Poly:** core polynomial types, term orders, and exponent vectors
- **Ps:** power-series support and coefficient helpers.
- **Structure:** algebraic interfaces and factories shared across modules (`RingElem`, `GcdRingElem`, `MonoidFactory`).
- **UFD:** [unique factorization domains](https://en.wikipedia.org/wiki/Unique_factorization_domain) algorithms—gcd variants, Hensel lifting, squarefree decompositions, quotient rings.
- **Util:** generic utilities (Cartesian products, list helpers, long iterables).
- **Vector:** basic linear algebra routines used by polynomial algorithms.
