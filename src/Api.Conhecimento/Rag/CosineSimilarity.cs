namespace Api.Conhecimento.Rag;

public static class CosineSimilarity
{
    public static float Calcular(ReadOnlySpan<float> a, ReadOnlySpan<float> b)
    {
        if (a.Length != b.Length)
        {
            throw new ArgumentException("Os vetores devem ter a mesma dimensão.");
        }

        double produto = 0, normaA = 0, normaB = 0;
        for (var i = 0; i < a.Length; i++)
        {
            produto += a[i] * b[i];
            normaA += a[i] * a[i];
            normaB += b[i] * b[i];
        }

        if (normaA == 0 || normaB == 0)
        {
            return 0f;
        }

        return (float)(produto / (Math.Sqrt(normaA) * Math.Sqrt(normaB)));
    }
}
