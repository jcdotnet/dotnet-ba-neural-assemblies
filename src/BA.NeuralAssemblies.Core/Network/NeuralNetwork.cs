using BA.NeuralAssemblies.Core.Activation;
using BA.NeuralAssemblies.Core.Learning;
using BA.NeuralAssemblies.Core.Learning.BA;

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

    // Previous activation values for each neuron in the network = Net(:,2) in MATLAB
    private readonly double[] _previousState = new double[networkSize];
    // Current activation values for each neuron in the network = Net(:,1) in MATLAB
    private readonly double[] _currentState = new double[networkSize];

    public double[] CurrentState => (double[])_currentState.Clone();


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

    public double[] CalculateRecurrentPotential()
    {
        double[] potential = new double[_previousState.Length];

        for (int k = 0; k < potential.Length; k++)
        {
            for (int i = 0; i < _previousState.Length; i++)
            {
                potential[k] += _networkWeights[i, k] * _previousState[i];
            }
        }

        return potential;
    }

    public void InitializeWeights(double[,]? inputWeights = null, double[,]? networkWeights = null)
    {
        if (inputWeights != null) CopyWeights(inputWeights, _inputWeights);
        if (networkWeights != null) CopyWeights(networkWeights, _networkWeights);
    }

    /// <summary>
    /// Executes one network cycle, calculating neuron activations and updating the weights
    /// </summary>
    /// <param name="input">The input pattern presented to the network.</param>
    /// <param name="learningRule">Learning rule used to update the network weights.</param>
    public void Cycle(double [] input, LearningRule learningRule)
    {
        // Calculate the input contribution for each neuron
        var inputPotential = CalculateInputPotential(input);

        // Calculate the total potential by adding the input and recurrent contributions
        double[] recurrentPotential = CalculateRecurrentPotential();
        double[] totalPotential = new double[inputPotential.Length];
        for (int i = 0; i < totalPotential.Length; i++)
        {
            totalPotential[i] = inputPotential[i] + recurrentPotential[i];
        }

        // Calculate the new neuron activations using the activation function
        for (int i = 0; i < totalPotential.Length; i++)
        {
            _currentState[i] = ActivationFunctions.Out(
                totalPotential[i], 
                SigmoidParameters.Sft, 
                SigmoidParameters.Slp
            );
        }

        // Update the network weights according to the learning rule
        switch (learningRule)
        {
            case LearningRule.BA:
                for (int i = 0; i < _previousState.Length; i++)
                {
                    for (int j = 0; j < _currentState.Length; j++)
                    {
                        _networkWeights[i, j] +=
                            BAParameters.Con * 
                            _previousState[i] *
                            _currentState[j] *
                            (_currentState[j] - BAParameters.Tau) * 
                            (BAParameters.Lambda - _currentState[j]);
                    }
                }
                break;
        }

        // Nature constraint is not applied because it is not implemented in the original C++ version
        // Store the new activations as the current and previous state for the next cycle
        for (int i = 0; i < _previousState.Length; i++)
        {
            _previousState[i] = _currentState[i];
        }
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
