using BA.NeuralAssemblies.Core.Learning;
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
        double[] input = [1.0, 0.5];

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
        double[] input = [1.0, 0.5];

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

    #region CalculateRecurrentPotential

    [Fact]
    public void CalculateRecurrentPotential_ShouldReturnZeros_WhenWeightsAreZeros()
    {
        // Arrange
        var network = new NeuralNetwork(inputSize: 2, networkSize: 3);

        // Act
        double[] result = network.CalculateRecurrentPotential();

        // Assert
        Assert.Equal([0.0, 0.0, 0.0], result);
    }

    #endregion

    #region Cycle

    [Fact]
    public void Cycle_ShouldCalculateExpectedActivations()
    {
        // Arrange
        var network = new NeuralNetwork(1, 2);

        double[,] inputWeights =
        {
            { 0.1, 0.2 }
        };

        network.InitializeWeights(inputWeights: inputWeights);

        double[] input = [1.0];

        // Act
        network.Cycle(input, LearningRule.BA);

        // Assert (expected values calculated using the original sigmoid function)
        Assert.Equal(0.0179862, network.CurrentState[0], 7); 
        Assert.Equal(0.0474259, network.CurrentState[1], 7);
    }

    #endregion
}
