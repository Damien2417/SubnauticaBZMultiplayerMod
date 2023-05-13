/// <summary>
/// Spreads data out to multiple text writers.
/// source: https://stackoverflow.com/questions/8823562/how-to-split-a-single-textwriter-into-multiple-outputs-in-net
/// </summary>
namespace ServerSubnautica.IO
{
    public class MultiTextWriter : System.IO.TextWriter
    {
        private System.Collections.Generic.List<System.IO.TextWriter> writers = new System.Collections.Generic.List<System.IO.TextWriter>();
        private System.IFormatProvider formatProvider = null;
        private System.Text.Encoding encoding = null;

        #region TextWriter Properties
        public override System.IFormatProvider FormatProvider
        {
            get
            {
                System.IFormatProvider formatProvider = this.formatProvider;
                if (formatProvider == null)
                {
                    formatProvider = base.FormatProvider;
                }
                return formatProvider;
            }
        }

        public override string NewLine
        {
            get { return base.NewLine; }

            set
            {
                foreach (System.IO.TextWriter writer in this.writers)
                {
                    writer.NewLine = value;
                }

                base.NewLine = value;
            }
        }


        public override System.Text.Encoding Encoding
        {
            get
            {
                System.Text.Encoding encoding = this.encoding;

                if (encoding == null)
                {
                    encoding = System.Text.Encoding.Default;
                }

                return encoding;
            }
        }

        #region TextWriter Property Setters

        MultiTextWriter SetFormatProvider(System.IFormatProvider value)
        {
            this.formatProvider = value;
            return this;
        }

        MultiTextWriter SetEncoding(System.Text.Encoding value)
        {
            this.encoding = value;
            return this;
        }
        #endregion TextWriter Property Setters
        #endregion TextWriter Properties


        #region Construction/Destruction
        public MultiTextWriter() { }
        public MultiTextWriter(System.Collections.Generic.IEnumerable<System.IO.TextWriter> writers)
        {
            this.Clear();
            this.AddWriters(writers);
        }
        #endregion Construction/Destruction

        #region Public interface
        public MultiTextWriter Clear()
        {
            this.writers.Clear();
            return this;
        }

        public MultiTextWriter AddWriter(System.IO.TextWriter writer)
        {
            this.writers.Add(writer);
            return this;
        }

        public MultiTextWriter AddWriters(System.Collections.Generic.IEnumerable<System.IO.TextWriter> writers)
        {
            this.writers.AddRange(writers);
            return this;
        }
        #endregion Public interface

        #region TextWriter methods

        public override void Close()
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Close();
            }
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                if (disposing)
                {
                    writer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Flush();
            }

            base.Flush();
        }

        public override void Write(bool value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(char value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(char[] buffer)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(buffer);
            }
        }

        public override void Write(decimal value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(double value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(float value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(int value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(long value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(object value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(string value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(uint value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(ulong value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        public override void Write(string format, object arg0)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(format, arg0);
            }

        }

        public override void Write(string format, params object[] arg)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(format, arg);
            }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(buffer, index, count);
            }
        }

        public override void Write(string format, object arg0, object arg1)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(format, arg0, arg1);
            }
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.Write(format, arg0, arg1, arg2);
            }
        }

        public override void WriteLine()
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine();
            }
        }

        public override void WriteLine(bool value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(char value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(char[] buffer)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(buffer);
            }
        }

        public override void WriteLine(decimal value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(double value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(float value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(int value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(long value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(object value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(string value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(uint value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(ulong value)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(value);
            }
        }

        public override void WriteLine(string format, object arg0)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(format, arg0);
            }
        }

        public override void WriteLine(string format, params object[] arg)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(format, arg);
            }
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(buffer, index, count);
            }
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(format, arg0, arg1);
            }
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            foreach (System.IO.TextWriter writer in this.writers)
            {
                writer.WriteLine(format, arg0, arg1, arg2);
            }
        }
        #endregion TextWriter methods
    }
}