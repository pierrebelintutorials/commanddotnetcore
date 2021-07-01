using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAppConsole
{
    public class CommandLineOptions
    {
        [Option('a', "apikey", Required = true, HelpText = "Binance API Key")]
        public string ApiKey
        {
            get; set;
        }

        [Option('s', "symbol", Required = true, HelpText = "Symbol")]
        public string Symbol
        {
            get; set;
        }
    }
}
