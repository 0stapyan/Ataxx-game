namespace Ataxx.Input.Console;

using System;
using System.Text.RegularExpressions;

internal static class CoordinateParser
{
    public static bool TryParse(string input, out int x, out int y)
    {
        x = 0;
        y = 0;
        
        string cleanInput = input.Trim().ToUpper();
        
        if (string.IsNullOrEmpty(cleanInput) || cleanInput.Length < 2)
        {
            return false;
        }
        
        char colChar = cleanInput[0];
        if (!char.IsLetter(colChar))
        {
            return false;
        }
        x = colChar - 'A';
        
        string rowString = cleanInput.Substring(1);
        if (!int.TryParse(rowString, out int rowNum))
        {
            return false;
        }
        y = rowNum - 1;
        
        if (x < 0 || y < 0)
        {
            return false;
        }

        return true;
    }
}
