﻿using System.Collections.Generic;

namespace Crayon
{
    class ErrorPrinter
    {
        public static void ShowErrors(IList<Common.Error> errors)
        {
            List<string> errorLines = new List<string>();
            foreach (Common.Error error in errors)
            {
                string errorString;
                if (error.Line != 0)
                {
                    errorString = error.FileName + " Line " + error.Line + ", Column " + error.Column + ": " + error.Message;
                }
                else if (error.FileName != null)
                {
                    errorString = error.FileName + ": " + error.Message;
                }
                else
                {
                    errorString = error.Message;
                }
            }

            string finalErrorString = string.Join("\n\n", errorLines);

            Common.ConsoleWriter.Print(Common.ConsoleMessageType.PARSER_ERROR, finalErrorString);
        }
    }
}
