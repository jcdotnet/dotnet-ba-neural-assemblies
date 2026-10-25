namespace BA.NeuralAssemblies.Core.Activation;

/// <summary>
/// Provides activation functions used by the neural network
/// </summary>
public static class ActivationFunctions
{
    /// <summary>
    ///     Calculates the neuron's acivity using the sigmoid function
    ///     defined by the original MATLAB Out.m and C++ implementation
    /// </summary>
    /// <param name="input">
    ///     The total input received by the neuron from external inputs
    ///     and connections from other neurons.
    /// </param>
    /// <param name="sft">
    ///     The offset of the sigmoid. 
    ///     It determines the input value at which the neuron's activity is 0.5.
    /// </param>
    /// <param name="slp">
    ///     The slope of the sigmoid. 
    ///     It determines how quickly the neuron's activity changes as the input changes
    /// </param>
    /// <returns>The neuron's activity, a value between 0 and 1</returns>
    public static double Out(double input, double sft, double slp)
    {
        return 1.0 / (1.0 + Math.Exp(slp * (sft - input)));
    }

}
