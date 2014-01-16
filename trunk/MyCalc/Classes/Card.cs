using System;
using System.Diagnostics;

namespace MyCalc.Classes {
	[DebuggerDisplay("S={Suit} V={Value}")]
	public struct Card {
		public UInt16 Value;
		public UInt16 Suit;
		public bool IsEmpty { get { return Value == 0 && Suit == 0; } }
		
		public Card(UInt16 value, UInt16 suit) {
			Value = value;
			Suit = suit;
		}
	}
}