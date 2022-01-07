//Stathis Georgiou ©2021
using System;
using System.Collections.Generic;

namespace Diadrasis.Mnesias {

	[Serializable]
	public class ChordInsJsonData
	{
		public string inpBodyMaterial, inpWayOfPlaying, inputChordCount, inpChordCountTriangle;
		public List<string> inpChordLength, inpChordTone, inpChordVolume, inpChordLengthTriangle, inpChordToneTriangle, inpChordVolumeTriangle;
	}

}
