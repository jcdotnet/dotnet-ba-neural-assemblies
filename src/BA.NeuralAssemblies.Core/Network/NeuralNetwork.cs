namespace BA.NeuralAssemblies.Core.Network;

/// <summary>
/// Represents a neural network
/// </summary>
/// <remarks>
/// Synaptic weights are represented using matrices rather than
/// neuron objects in order to follow the original implementation
/// </remarks>
public class NeuralNetwork(int inputSize, int networkSize)
{
    // Determine how much each input value (rows) contributes to each neuron in the network (cols)
    private readonly double[,] _inputWeights = new double[inputSize, networkSize];

    // Determine how much each neuron in the network contributes to every other neuron in the network
    private readonly double[,] _networkWeights = new double[networkSize, networkSize];

    /// <summary>
    /// Calculates the input potential for each neuron in the network
    /// </summary>
    /// <param name="input">The input pattern presented to the network</param>
    /// <returns>The input potential for each neuron in the network</returns>
    public double[] CalculateInputPotential(double[] input)
    {
        int networkSize = _inputWeights.GetLength(1);
        int inputSize = _inputWeights.GetLength(0);

        if (input.Length != inputSize)
            throw new ArgumentException("Input length must match the number of input neurons.", nameof(input));

        double[] inputPotential = new double[networkSize];

        for (int k = 0; k < networkSize; k++)
        {
            for (int i = 0; i < inputSize; i++)
            {
                inputPotential[k] += input[i] * _inputWeights[i, k];
            }
        }

        return inputPotential;
    }

    public void InitializeWeights(double[,]? inputWeights = null, double[,]? networkWeights = null)
    {
        if (inputWeights != null) CopyWeights(inputWeights, _inputWeights);
        if (networkWeights != null) CopyWeights(networkWeights, _networkWeights);
    }

    public void Cycle()
    {
        // TO-DO
    }

    #region private

    private void CopyWeights(double[,] source, double[,] target)
    {
        if (source.GetLength(0) != target.GetLength(0) ||
            source.GetLength(1) != target.GetLength(1))
        {
            throw new ArgumentException("Weights must match the network dimensions.", nameof(source));
        }
        Array.Copy(source, target, source.Length);
    }

    #endregion

}
