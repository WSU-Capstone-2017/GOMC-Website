namespace Project.Models.Gomc
{
	/// <summary>
	///     Settings for the simulation output.
	/// </summary>
	public class ConfigOutputSection
	{
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

		public ConfigOutputSection()
		{
			
		}
		public ConfigOutputSection(string outputName, FreqInput coordinatesFreq, FreqInput restartFreq, FreqInput consoleFreq, FreqInput blockAverageFreq, FreqInput histogramFreq, string distName, string histName, uint runNumber, char runLetter, uint sampleFreq, OutBoolean outEnergy, OutBoolean outPressure, OutBoolean outMolNumber, OutBoolean outDensity, OutBoolean outVolume, OutBoolean outSurfaceTension)
		{
			OutputName = outputName;
			CoordinatesFreq = coordinatesFreq;
			RestartFreq = restartFreq;
			ConsoleFreq = consoleFreq;
			BlockAverageFreq = blockAverageFreq;
			HistogramFreq = histogramFreq;
			DistName = distName;
			HistName = histName;
			RunNumber = runNumber;
			RunLetter = runLetter;
			SampleFreq = sampleFreq;
			OutEnergy = outEnergy;
			OutPressure = outPressure;
			OutMolNumber = outMolNumber;
			OutDensity = outDensity;
			OutVolume = outVolume;
			OutSurfaceTension = outSurfaceTension;
		}
	}
}