using BA.NeuralAssemblies.Core.Activation;

namespace BA.NeuralAssemblies.Tests.Activation;

public class ActivationFunctionTests
{

    // Expected values obtained by running Out.m in MATLAB
    // The original out.m file is located in legacy/matlab 
    [Theory]
    [InlineData(0.0, 0.006692850924285)] 
    [InlineData(0.5, 0.500000000000000)]
    [InlineData(1.0, 0.993307149075715)]
    public void Out_ShouldReturnExpectedValues(double input, double expected)
    {
        // Act
        double result = ActivationFunctions.Out(input, sft: 0.5, slp: 10.0);

        // Assert
        Assert.Equal(expected, result, 5);
    }
}
