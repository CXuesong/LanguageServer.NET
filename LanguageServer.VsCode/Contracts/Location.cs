using System;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Represents a location inside a resource, such as a line inside a text file.
    /// </summary>
    public struct Location : IEquatable<Location>
    {
        public Range Range { get; set; }

        public Uri Uri { get; set; }

        /// <inheritdoc />
        public bool Equals(Location other)
        {
            return Range.Equals(other.Range) && Uri == other.Uri;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj is Location && Equals((Location) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Range.GetHashCode() * 397) ^ (Uri != null ? Uri.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Location x, Location y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Location x, Location y)
        {
            return !x.Equals(y);
        }
    }

    /// <summary>
    /// A range in a text document expressed as (zero-based) start and end positions.
    /// A range is comparable to a selection in an editor.
    /// Therefore the end position is exclusive.
    /// </summary>
    public struct Range : IEquatable<Range>
    {
        public Range(Position start, Position end)
        {
            Start = start;
            End = end;
        }

        public Range(int startLine, int startCharacter, int endLine, int endCharacter)
        {
            Start = new Position(startLine, startCharacter);
            End = new Position(endLine, endCharacter);
        }

        public Position Start { get; set; }

        public Position End { get; set; }

        /// <inheritdoc />
        public override string ToString() => $"{Start}-{End}";

        /// <inheritdoc />
        public bool Equals(Range other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Range && Equals((Range) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Start.GetHashCode() * 397) ^ End.GetHashCode();
            }
        }

        public static bool operator ==(Range x, Range y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Range x, Range y)
        {
            return !x.Equals(y);
        }
    }

    /// <summary>
    /// Position in a text document expressed as zero-based line and character offset.
    /// A position is between two characters like an 'insert' cursor in a editor.
    /// </summary>
    public struct Position : IEquatable<Position>, IComparable<Position>
    {
        public Position(int line, int character)
        {
            Line = line;
            Character = character;
        }

        public int Line { get; set; }

        public int Character { get; set; }

        /// <inheritdoc />
        public override string ToString() => $"({Line},{Character})";

        /// <inheritdoc />
        public bool Equals(Position other)
        {
            return Character == other.Character && Line == other.Line;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj is Position && Equals((Position) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Line * 397) ^ Character;
            }
        }

        /// <inheritdoc />
        public int CompareTo(Position other)
        {
            var lineCmp = Line.CompareTo(other.Line);
            if (lineCmp != 0) return lineCmp;
            return Character.CompareTo(other.Character);
        }

        public static bool operator == (Position x, Position y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Position x, Position y)
        {
            return !x.Equals(y);
        }

        public static bool operator >(Position x, Position y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <(Position x, Position y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >=(Position x, Position y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static bool operator <=(Position x, Position y)
        {
            return x.CompareTo(y) <= 0;
        }
    }
}
