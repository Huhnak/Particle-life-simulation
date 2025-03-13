using UnityEngine;

public class OneProperty : MonoBehaviour
{
    public struct value
    {
        public value(int origin, int target)
        {
            this.origin = origin;
            this.target = target;
        }
        public int origin;
        public int target;
    }
    public value Value { get; set; }

}
