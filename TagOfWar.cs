/*
	Tug of war

 * Tug of war is a contest of brute strength, where two teams of people pull in opposite
 * directions on a rope. The team that succeeds in pulling the rope in their direction is
 * declared the winner.
 * A tug of war is being arranged for the office picnic. The picnickers must be fairly
 * divided into two teams. Every person must be on one team or the other, the number of
 * people on the two teams must not differ by more than one, and the total weight of the
 * people on each team should be as nearly equal as possible.
 * Input
 * The input begins with a single positive integer on a line by itself indicating the number
 * of test cases following, each described below and followed by a blank line.
 * The first line of each case contains n, the number of people at the picnic. Each of the
 * next n lines gives the weight of a person at the picnic, where each weight is an integer
 * between 1 and 450. There are at most 100 people at the picnic.
 * Finally, there is a blank line between each two consecutive inputs.
 * Output
 * For each test case, your output will consist of a single line containing two numbers: the
 * total weight of the people on one team, and the total weight of the people on the other
 * team. If these numbers differ, give the smaller number first.
 * The output of each two consecutive cases will be separated by a blank line.
 * Sample Input
 * 1
 * 3
 * 100
 * 90
 * 200
 * Sample Output
 * 190 200
 */
namespace TagOfWar
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json;

	public static class TagOfWar
	{
		private static readonly List<Solution> Solutions = new List<Solution>();

		private static readonly List<int> Numbers = new List<int>() { 4, 10, 7, 14, 8, 6, 11, 15, 13, -3 };

		// private static readonly List<int> Numbers = new List<int>() { -1, -3, -5, -6, 4, 11, 17, 23, 27, 19, 4 };

		public static void Run()
		{
			var subSetElementCount = Numbers.Count % 2 == 0 ? (Numbers.Count / 2) : ((Numbers.Count - 1) / 2);

			foreach (var number in Numbers)
			{
				var otherNumbers = GetOtherNumbers(number, Numbers);
				AddSolutions(subSetElementCount, otherNumbers);
			}

			var minDifferenceSolution = Solutions.FirstOrDefault(x => x.Difference == Solutions.Min(s => s.Difference));
			Console.WriteLine($"Min Difference Soltion {minDifferenceSolution.Key}");
		}

		private static void AddSolutions(int subSetElementCount, List<int> otherNumbers, Solution currentSolution = null)
		{
			foreach (var otherNumber in otherNumbers)
			{
				currentSolution = currentSolution ?? new Solution();

				if (currentSolution.SubSetOne.Count == subSetElementCount - 1)
				{
					currentSolution.SubSetOne.Add(otherNumber);
					currentSolution.UpdateSolutionWithKeyAndDifference();
					if (!Solutions.IsExists(currentSolution))
					{
						Solutions.Add(JsonConvert.DeserializeObject<Solution>(JsonConvert.SerializeObject(currentSolution)));
					}
					currentSolution.SubSetOne.RemoveAt(currentSolution.SubSetOne.Count - 1);
				}
				else
				{
					currentSolution.SubSetOne.Add(otherNumber);
					var updatedOtherNumbers = UpdateOtherNumbers(otherNumbers, GetOtherNumbers(otherNumber, Numbers));
					AddSolutions(subSetElementCount, updatedOtherNumbers, currentSolution);
					currentSolution.SubSetOne.RemoveAt(currentSolution.SubSetOne.Count - 1);
				}
			}
		}

		private static List<int> GetOtherNumbers(int number, List<int> allNumbers)
		{
			return allNumbers.Where(x => x != number).ToList();
		}

		private static List<int> UpdateOtherNumbers(List<int> existingOtherNumbers, List<int> newOtherNumbers)
		{
			return existingOtherNumbers.Where(x => newOtherNumbers.Contains(x)).ToList();
		}

		private static void UpdateSolutionWithKeyAndDifference(this Solution solution)
		{
			solution.SubSetTwo = GetSubsetTwoNumbers(solution.SubSetOne, Numbers);
			solution.Difference = Math.Abs(solution.SubSetOne.Sum() - solution.SubSetTwo.Sum());
			solution.Key = solution.GetSolutionKey();
		}

		private static bool IsExists(this List<Solution> solutions, Solution solution)
		{
			return solutions.Count(p => p.SubSetOne.Count(x => solution.SubSetOne.Any(y => y == x)) == solution.SubSetOne.Count) > 0;
		}

		private static List<int> GetSubsetTwoNumbers(List<int> subSetOneNumbers, List<int> allNumbers)
		{
			return allNumbers.Where(x => !subSetOneNumbers.Contains(x)).ToList();
		}

		private static string GetSolutionKey(this Solution solution)
		{
			var subSetOnekey = string.Join(",", solution.SubSetOne.Select(x => $"{x}"));
			var subSetTwokey = string.Join(",", solution.SubSetTwo.Select(x => $"{x}"));
			return $"({subSetOnekey}) and ({subSetTwokey})";
		}
	}

	public class Solution
	{
		public string Key { get; set; }

		public List<int> SubSetOne { get; set; } = new List<int>();

		public List<int> SubSetTwo { get; set; } = new List<int>();

		public int Difference { get; set; }
	}
}
