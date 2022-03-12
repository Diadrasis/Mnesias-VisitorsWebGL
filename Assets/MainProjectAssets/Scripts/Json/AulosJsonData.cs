//Stathis Georgiou ©2021
using System;
using System.Collections.Generic;

namespace Diadrasis.Mnesias {

	[Serializable]
	public class AulosJsonData
	{
		public string inpTubeLength, inpBoreDiameter, inpMouthpieceLength, inpShortestToneholeHeight, inputToneholesCount;
		public List<string> holesDiams, holesDists;
	}

}
