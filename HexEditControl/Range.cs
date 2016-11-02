﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zodiacon.HexEditControl {
	public struct Range : IEquatable<Range> {
		public bool IsEmpty => Start > End;

		public long Start { get; }
		public long End { get; }
		public long Count => End - Start + 1;

		private Range(long start, long end)
			 : this() {
			Start = start;
			End = end;
		}

		public static Range FromStartToEnd(long start, long end) {
			return new Range(start, end);
		}

		public static Range FromStartAndCount(long start, long count) {
			return FromStartToEnd(start, start + count - 1);
		}

		public bool ContainedEntirelyWithin(Range other) {
			return GetIntersection(other) == this;
		}

		public bool ContainsEntirely(Range other) {
			return GetIntersection(other) == other;
		}

		public bool Contains(long value) {
			var result = true;
			result &= Start.CompareTo(value) <= 0;
			result &= End.CompareTo(value) >= 0;

			return result;
		}

		public Range GetIntersection(Range other) {
			if (!Intersects(other))
				return new Range();

			long start, end;

			start = Start.CompareTo(other.Start) >= 0 ? Start : other.Start;

			end = End.CompareTo(other.End) < 0 ? End : other.End;

			return new Range(start, end);
		}

		public bool Intersects(Range other) {
			var hasClosedInterval = Start.CompareTo(other.End) <= 0 && other.End.CompareTo(End) <= 0;

			var hasOpenInterval =
				 (Start.CompareTo(other.End) <= 0) || (End.CompareTo(other.Start) >= 0) ||
				 (other.Start.CompareTo(End) <= 0) || (other.End.CompareTo(Start) >= 0);

			return hasClosedInterval || hasOpenInterval;
		}

		public override string ToString() {
			return $"{Start.ToString()}..{End.ToString()}";
		}

		public override int GetHashCode() {
			return Start.GetHashCode() ^ End.GetHashCode();
		}

		public static bool operator ==(Range a, Range b) {
			return a.Start.Equals(b.Start) && a.End.Equals(b.End);
		}

		public static bool operator !=(Range a, Range b) {
			return !(a == b);
		}

		public override bool Equals(object obj) {
			if (!(obj is Range))
				return false;

			return (this == (Range)obj);
		}

		public bool Equals(Range other) {
			return this == other;
		}
	}
}
