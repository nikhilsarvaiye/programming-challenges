/*
	Little Bishops

 * A bishop is a piece used in the game of chess which can only move diagonally from
 * its current position. Two bishops attack each other if one is on the path of the other.
 * In the figure below, the dark squares represent the reachable locations for bishop B1
 * from its current position. Bishops B1 and B2 are in attacking position, while B1 and
 * B3 are not. Bishops B2 and B3 are also in non-attacking position.
 * 
 * Given two numbers n and k, determine the number of ways one can put k bishops
 * on an n × n chessboard so that no two of them are in attacking positions.
 * Input
 * The input file may contain multiple test cases. Each test case occupies a single line in
 * the input file and contains two integers n(1 ≤ n ≤ 8) and k(0 ≤ k ≤ n2).
 * A test case containing two zeros terminates the input.
 *
 * Output
 * For each test case, print a line containing the total number of ways one can put the
 * given number of bishops on a chessboard of the given size so that no two of them lie in
 * attacking positions. You may safely assume that this number will be less than 1015.
 *
 * Sample Input
 * 8 6
 * 4 4
 * 0 0
 *
 * Sample Output
 * 5599888
 * 260
 */
namespace EYC.Solutions.Anticipate.Planning.Tools.Debug
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json;

	public static class BishopProblem
	{
		private static readonly int N = 4;
		private static readonly int K = 4;
		private static readonly Dictionary<string, List<Square>> UniquePositions = new Dictionary<string, List<Square>>();

		public static void FindAllUniquePositions()
		{
			for (int i = 0; i < N; i++)
			{
				for (int j = 0; j < N; j++)
				{
					var square = new Square(i, j);
					var attackingSquares = GetBishopAttackingSquares(square);
					var nonAttackingSqaures = GetBishopNonAttackingSquares(square, attackingSquares);
					AddNonAttackingAllBishopPositions(nonAttackingSqaures, new List<Square>() { square });
				}
			}

			Console.WriteLine($"Total Positions {UniquePositions.Distinct().Count()}");
		}

		private static List<Square> AddNonAttackingAllBishopPositions(List<Square> nonAttackingSquares, List<Square> allSquares)
		{
			foreach (var nonAttackingSquare in nonAttackingSquares)
			{
				if (allSquares.Count == K - 1)
				{
					allSquares.Add(nonAttackingSquare);
					allSquares.AddToUniquePositions();
					allSquares.RemoveAt(allSquares.Count - 1);
				}
				else
				{
					allSquares.Add(nonAttackingSquare);
					var updatedNonAttackingSquares = UpdateNonAttackingSquares(nonAttackingSquares, GetBishopAttackingSquares(nonAttackingSquare));
					allSquares = AddNonAttackingAllBishopPositions(updatedNonAttackingSquares, allSquares);
					allSquares.RemoveAt(allSquares.Count - 1);
				}
			}

			return allSquares;
		}

		private static List<Square> GetAllSquares()
		{
			var squares = new List<Square>();

			for (int i = 0; i < N; i++)
			{
				for (int j = 0; j < N; j++)
				{
					squares.Add(new Square(i, j));
				}
			}

			return squares;
		}

		private static List<Square> GetBishopAttackingSquares(Square square)
		{
			var squares = new List<Square>();

			// Case 1. Increase both row and column index
			var rowIndex = square.RowIndex;
			var columnIndex = square.ColumnIndex;
			while (rowIndex < N && columnIndex < N)
			{
				squares.Add(new Square(rowIndex++, columnIndex++));
			}

			// Case 2. Decrease both row and column index
			rowIndex = square.RowIndex;
			columnIndex = square.ColumnIndex;
			while (rowIndex >= 0 && columnIndex >= 0)
			{
				squares.Add(new Square(rowIndex--, columnIndex--));
			}

			// Case 3. Increase row and Decrease column index
			rowIndex = square.RowIndex;
			columnIndex = square.ColumnIndex;
			while (rowIndex < N && columnIndex >= 0)
			{
				squares.Add(new Square(rowIndex++, columnIndex--));
			}

			// Case 4. Decrease row and Increase column index
			rowIndex = square.RowIndex;
			columnIndex = square.ColumnIndex;
			while (rowIndex >= 0 && columnIndex < N)
			{
				squares.Add(new Square(rowIndex--, columnIndex++));
			}

			return squares;
		}

		private static List<Square> GetBishopNonAttackingSquares(Square square, List<Square> attackingSquares)
		{
			var allSquares = GetAllSquares();

			return allSquares.Where(x => !attackingSquares.Any(y => y.RowIndex == x.RowIndex && y.ColumnIndex == x.ColumnIndex)).ToList();
		}

		private static List<Square> UpdateNonAttackingSquares(List<Square> nonAttackingSquares, List<Square> attackingSquares)
		{
			return nonAttackingSquares.Where(x => !attackingSquares.Any(y => y.RowIndex == x.RowIndex && y.ColumnIndex == x.ColumnIndex)).ToList();
		}

		private static void AddToUniquePositions(this List<Square> allSquares)
		{
			if (!UniquePositions.IsExists(allSquares))
			{
				var key = GetUniquePositionKey(allSquares);
				Console.WriteLine($"{key} with Count {UniquePositions.Count}");
				var uniqueSquaresPositions = JsonConvert.DeserializeObject<List<Square>>(JsonConvert.SerializeObject(allSquares));
				UniquePositions.Add(key, uniqueSquaresPositions);
			}
		}

		private static string GetUniquePositionKey(List<Square> squares)
		{
			return string.Join("-", squares.Select(x => $"{x.RowIndex}{x.ColumnIndex}"));
		}

		private static bool IsExists(this Dictionary<string, List<Square>> uniquePositions, List<Square> squares)
		{
			return uniquePositions.Count(p => p.Value.Count(x => squares.Any(y => y.RowIndex == x.RowIndex && y.ColumnIndex == x.ColumnIndex)) == squares.Count) > 0;
		}
	}
}
