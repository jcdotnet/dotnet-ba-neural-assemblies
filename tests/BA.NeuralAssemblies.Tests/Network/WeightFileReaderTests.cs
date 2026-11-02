using BA.NeuralAssemblies.Core.Network;

namespace BA.NeuralAssemblies.Tests.Network;

public class WeightFileReaderTests
{
    [Fact]
    public void Read_ShouldReturnExpectedDimensions()
    {
        // Arrange
        // Act
        double[,] inputWeights = WeightFileReader.Read("conexA.dat", 300, 600);
        double[,] networkWeights = WeightFileReader.Read("conexNeu.dat", 600, 600);

        // Assert
        Assert.Equal(300, inputWeights.GetLength(0));
        Assert.Equal(600, inputWeights.GetLength(1));

        Assert.Equal(600, networkWeights.GetLength(0));
        Assert.Equal(600, networkWeights.GetLength(1));
    }

    [Fact]
    public void Read_ShouldReturnExpectedValues()
    {
        // Arrange
        // Act
        double[,] inputWeights = WeightFileReader.Read("conexA.dat", 300, 600);
        double[,] networkWeights = WeightFileReader.Read("conexNeu.dat", 600, 600);

        // Assert
        Assert.Equal(0.0, inputWeights[0, 0]);
        Assert.Equal(0.0, inputWeights[0, 1]);
        Assert.Equal(0.0, inputWeights[0, 2]);

        Assert.Equal(0.0, networkWeights[0, 0]);
        Assert.Equal(0.0, networkWeights[0, 1]);
        Assert.Equal(0.0, networkWeights[0, 2]);
    }
}
