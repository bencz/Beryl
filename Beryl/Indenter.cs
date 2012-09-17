/** \file
 *  Defines the \c Indenter class, which handles indentation issues in output text files.
 *
 *  \note This file was copied from the Braceless sources and the namespace was renamed to \c Beryl.
 */

using System;                           // IDisposable

namespace Beryl
{
    /** The \c Indenter class transparently prefixes indentation so that the client does not need to worry about this. */
    public class Indenter
    {
        /** The stream to write to. */
        private System.IO.StreamWriter _writer;
        /** The current level of indentation (0 = none). */
        private int _level = 0;
        /** Tracks whether we're at the beginning of a line so that indentation needs to be output. */
        private bool _newline = true;

        /** The size of each indent (in spaces). */
        private string _indent = "    ";
        public int Size
        {
            get { return _indent.Length; }
            set { _indent = new string(' ', value); }
        }

        /** Constructor for the \c Indenter class. */
        public Indenter(string filename, System.Text.Encoding encoding)
        {
            _writer = new System.IO.StreamWriter(filename, false, encoding);
        }

        ~Indenter()
        {
            if (_writer != null)
                throw new BerylError("Beryl.Indenter.Close() not called!");
        }

        public void Close()
        {
            if (_writer != null)
            {
                _writer.Close();
                _writer = null;
            }
            _indent = null;
        }

        /** Helper function that writes \c _level levels of indentation if at the beginning of a new line. */
        private void WriteIndent()
        {
            if (!_newline)
                return;

            _newline = false;
            for (int i = 0; i < _level; i += 1)
            {
                _writer.Write(_indent);
            }
        }

        /** Increments the number of levels of indentation to be output. */
        public void Indent()
        {
            _level += 1;
        }

        /** Decrements the number of levels of indentation to be output. */
        public void Dedent()
        {
            if (_level <= 0)
                throw new BerylError("Dedent() without Indent()");
            _level -= 1;
        }

#if false
        public void Start(string text)
        {
            _writer.Write(text);
        }

        public void StartLine(string text)
        {
            _writer.WriteLine(text);
        }
#endif

        /** Writes the specified character after having ensured the line is properly indented. */
        public void Write(char text)
        {
            WriteIndent();
            _writer.Write(text);
        }

        /** Writes the specified string after having ensured the line is properly indented. */
        public void Write(string text)
        {
            WriteIndent();
            _writer.Write(text);
        }

        /** Writes the specified character after having ensured the line is properly indented and then terminates the line. */
        public void WriteLine(char text)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(text);
        }

        /** Writes the specified string after having ensured the line is properly indented and then terminates the line. */
        public void WriteLine(string text)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(text);
        }

        /** Writes a linefeed character after having ensured the line is properly indented. */
        public void WriteLine()
        {
            /* technically speaking, this call to \c WriteIndent() is superflous, but it harms nobody and it is consistent. */
            WriteIndent();
            _newline = true;
            _writer.WriteLine();
        }

        public void WriteLine(string format, object arg1)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(format, arg1);
        }

        public void WriteLine(string format, object arg1, object arg2)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(format, arg1, arg2);
        }

        public void WriteLine(string format, object arg1, object arg2, object arg3)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(format, arg1, arg2, arg3);
        }

        public void WriteLine(string format, object arg1, object arg2, object arg3, object arg4)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(format, arg1, arg2, arg3, arg4);
        }
    }
}

