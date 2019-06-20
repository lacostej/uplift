// --- BEGIN LICENSE BLOCK ---
/*
 * Copyright (c) 2019-present WeWantToKnow AS
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
// --- END LICENSE BLOCK ---


using Uplift.Common;
using Uplift.Packages;
using Uplift.Schemas;
using System.Collections.Generic;
using System.Text;

namespace Uplift.DependencyResolution
{
	public class Conflict
	{
		public DependencyDefinition requirement;
		public List<PossibilitySet> possibilities;
		public DependencyGraph activated;

		public Conflict(DependencyDefinition requirement, List<PossibilitySet> possibilities, DependencyGraph activated)
		{
			this.requirement = requirement;
			this.possibilities = possibilities;
			this.activated = activated;
		}

		override public string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(requirement.Name + " " + requirement.Version + " conflicts with state :");
			sb.AppendLine(activated.ToString());
			return sb.ToString();
		}

		/* 
				public static void CheckForConflict(State currentState)
				{
					//Check possibilities
					if (currentState.possibilities.Count > 1)
					{
						// Not in possibility state
					}
					else if (currentState.possibilities.Count == 0)
					{
						// Conflict !
					}
					else
					{
						// no conflict
					}

					//returns a conflict item
					//Empty if no conflicts
				}

				// struct UnwindDetails
				//--> Unwinding in DependencyBacktracking.cs ??
				//				   DependencyUnwinding.cs    ??
		*/
	}
}