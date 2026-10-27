using BA.NeuralAssemblies.Core.Network;

namespace BA.NeuralAssemblies.Tests.Network;

public class NeuralNetworkTests
{
    #region CalculateInputPotential

    [Fact]
    public void CalculateInputPotential_ShouldReturnZeros_WhenWeightsAreZeros()
    {
        // Arrange
        var network = new NeuralNetwork(inputSize: 2, networkSize: 3);
        double[] input = { 1.0, 0.5 };

        // Act
        double[] result = network.CalculateInputPotential(input);

        // Assert
        Assert.Equal([0.0, 0.0, 0.0], result);
    }

    [Fact]
    public void CalculateInputPotential_ShouldReturnExpectedValues_WhenWeightsAreNotZeros()
    {
        // Arrange
        var network = new NeuralNetwork(inputSize: 2, networkSize: 3);
        double[] input = { 1.0, 0.5 };

        double[,] inputWeights =
        {
            { 0.2, 0.4, 0.6 },
            { 0.5, 0.1, 0.3 }
        };

        network.InitializeWeights(inputWeights: inputWeights);

        double[] expected = { 0.45, 0.45, 0.75 };

        // Act
        double[] result = network.CalculateInputPotential(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion
}
