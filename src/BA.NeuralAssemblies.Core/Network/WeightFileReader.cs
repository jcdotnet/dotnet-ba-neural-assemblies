using System.Globalization;

namespace BA.NeuralAssemblies.Core.Network;

public class WeightFileReader
{
    public static double[,] Read(string filePath, int rows, int columns)
    {      
        string content;    
        try
        {
            content = File.ReadAllText(filePath);
        }
        catch (Exception exception)
        {
            throw new IOException($"An error ocurred reading {filePath}.", exception);
        }

        if (string.IsNullOrEmpty(content)) throw new InvalidDataException("The weights file is empty.");

        string[] values = content.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries);


        if (values.Length != rows * columns)
        {
            throw new InvalidDataException($"Expected {rows * columns} values, but found {values.Length}.");
        }

        double[,] matrix = new double[rows, columns];

        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Parse to double using the standard number format
                matrix[i, j] = double.Parse(values[index++], CultureInfo.InvariantCulture);
            }
        }

        return matrix;
    }

}
