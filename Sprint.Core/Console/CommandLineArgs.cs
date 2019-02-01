using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Sprint.Console
{
    public class CommandLineArgs : StringDictionary
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgs" /> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public CommandLineArgs(string[] args)
        {
            this.Extract(args);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgs" /> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public CommandLineArgs(string args)
        {
            if (String.IsNullOrEmpty(args))
            {
                Extract(new string[0]);
            }
            else
            {
                Regex Extractor = new Regex(@"(['""][^""]+['""])\s*|([^\s]+)\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                MatchCollection Matches;
                string[] Parts;

                // Get matches (first string ignored because Environment.CommandLine starts with program filename)
                Matches = Extractor.Matches(args);
                Parts = new string[Matches.Count - 1];
                for (int i = 1; i < Matches.Count; i++)
                {
                    Parts[i - 1] = Matches[i].Value.Trim();
                }

                Extract(Parts);
            }
        }

        /// <summary>
        /// Extracts the specified args.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private void Extract(string[] args)
        {
            base.Clear();
            Regex Spliter = new Regex(@"^([/-]|--){1}(?<name>\w+)([:=])?(?<value>.+)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            char[] TrimChars = { '"', '\'' };
            string Parameter = null;
            Match Part;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
            foreach (string arg in args)
            {
                Part = Spliter.Match(arg);
                if (!Part.Success)
                {
                    // Found a value (for the last parameter found (space separator))
                    if (Parameter != null)
                    {
                        this[Parameter] = arg.Trim(TrimChars);
                    }
                }
                else
                {
                    Parameter = Part.Groups["name"].Value;
                    base.Add(Parameter, Part.Groups["value"].Value.Trim(TrimChars));
                }
            }
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public bool TryGetValue(string key, out string value)
        {            
            IEnumerator e = this.GetEnumerator();

            while (e.MoveNext())
            {
                DictionaryEntry dic = (DictionaryEntry)e.Current;

                if (dic.Key.ToString().Trim() == key.Trim())
                {
                    value = dic.Value.ToString();
                    return true;
                }
            }

            value = "";
            return false;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string ret = "";

            foreach (string k in Keys)
            {
                ret += String.Format("{0}='{1}' \n", k, this[k]);
            }

            return ret;
        }
    }
}
