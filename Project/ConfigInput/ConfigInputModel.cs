using System;
using Project.Core;
using Project.Models.Gomc;

namespace Project.ConfigInput
{
	public class ConfigInputModel
	{
		// input section
		public Ensemble Ensemble { get; set; }
		public bool Restart { get; set; }
		public PrngType Prng { get; set; }
		public int? RandomSeed { get; set; }
		public ForceFieldType ParaType { get; set; }
		public string ParametersFileName { get; set; }
		public string[] Coordinates { get; set; }
		public string[] Structures { get; set; }

		// system section
		public double Pressure { get; set; }
		public double Temperature { get; set; }
		public double Rcut { get; set; }
		public double RcutLow { get; set; }
		public bool Lrc { get; set; }
		public ExcludeType Exclude { get; set; }
		public PotentialType Potential { get; set; }
		public double Rswitch { get; set; }
		public bool ElectroStatic { get; set; }
		public bool Ewald { get; set; }
		public bool CachedFourier { get; set; }
		public double Tolerance { get; set; }
		public double Dielectric { get; set; }
		public ulong? PressureCalc { get; set; }
		public double OneFourScaling { get; set; }
		public ulong RunSteps { get; set; }
		public ulong EqSteps { get; set; }
		public ulong AdjSteps { get; set; }
		public ResNameValue ChemPot { get; set; }
		public ResNameValue Fugacity { get; set; }
		public double DisFreq { get; set; }
		public double RotFreq { get; set; }
		public double IntraSwapFreq { get; set; }
		public double VolFreq { get; set; }
		public double SwapFreq { get; set; }
		public bool UseConstantArea { get; set; }
		public bool FixVolBox0 { get; set; }
		public BoxDimInput[] BoxDim { get; set; }
		public int CbmcFirst { get; set; }
		public int CbmcNth { get; set; }
		public int CbmcAng { get; set; }
		public int CbmcDih { get; set; }

		// output section

		public string OutputName { get; set; }
		public FreqInput CoordinatesFreq { get; set; }
		public FreqInput RestartFreq { get; set; }
		public FreqInput ConsoleFreq { get; set; }
		public FreqInput BlockAverageFreq { get; set; }
		public FreqInput HistogramFreq { get; set; }
		public string DistName { get; set; }
		public string HistName { get; set; }
		public uint RunNumber { get; set; }
		public char RunLetter { get; set; }
		public uint SampleFreq { get; set; }
		public OutBoolean OutEnergy { get; set; }
		public OutBoolean OutPressure { get; set; }
		public OutBoolean OutMolNumber { get; set; }
		public OutBoolean OutDensity { get; set; }
		public OutBoolean OutVolume { get; set; }
		public OutBoolean OutSurfaceTension { get; set; }

		public string AsJsonString() => JsonConv.ToJson(this);

		public static ConfigInputModel FromJson(string jsonString)
		{
			try
			{
				return JsonConv.ToObject<ConfigInputModel>(jsonString);
			}
			catch(Exception e)
			{
				return null;
			}
		}

	}
}