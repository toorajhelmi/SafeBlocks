using SafeBlocks;

namespace SafeBlocks.String
{
    public class _String
    {
        public string Value { private set; get; }

        public _String() { }
      
        public _String(string value) => Value = value;

        public _String Substring(int start, int length = -1)
        {
            if (length == -1)
            {
                length = Value.Length - start;
            }

            var sub = "";
            (new _For("SS", start, start + length, 10000, new _Action<int>(i => sub += Value[i]))).Do();
            return new _String(sub);
        }

        public override string ToString() => Value;
    }
}
