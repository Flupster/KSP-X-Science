using System.Collections.Generic;



namespace ScienceChecklist {
	/// <summary>
	/// An object that represents a ScienceExperiement in a given Situation.
	/// </summary>
	public sealed class ScienceInstance {
		#region FIELDS
		private readonly ScienceExperiment _experiment;
		private readonly Situation _situation;
		#endregion


		/// <summary>
		/// Creates a new instance of the Experiment class.
		/// </summary>
		/// <param name="experiment">The ScienceExperiment to be used.</param>
		/// <param name="situation">The Situation this experiment is valid in.</param>
		/// <param name="onboardScience">A collection of all onboard ScienceData.</param>
		public ScienceInstance( ScienceExperiment experiment, Situation situation, ScienceContext Sci )
		{
			_experiment = experiment;
			_situation = situation;
			ScienceSubject = null;
			Update( Sci );
		}

		#region PROPERTIES

		/// <summary>
		/// Gets the ScienceExperiment used by ResearchAndDevelopment.
		/// </summary>
		public ScienceExperiment    ScienceExperiment { get { return _experiment; } }
		/// <summary>
		/// Gets the Situation in which this experiment is valid.
		/// </summary>
		public Situation            Situation         { get { return _situation; } }
		
		/// <summary>
		/// Gets the ResearchAndDevelopment ID for this experiment.
		/// </summary>
		public string Id {
			get {
				return string.Format("{0}@{1}{2}{3}", ScienceExperiment.id, Situation.Body.Name, Situation.ExperimentSituation, (Situation.SubBiome ?? Situation.Biome ?? string.Empty).Replace(" ", ""));
			}
		}

		/// <summary>
		/// Gets the amount of science completed for this experiment.
		/// </summary>
		public float  CompletedScience { get; private set; }
		/// <summary>
		/// Gets a value indicating if this experiment has been unlocked in the tech tree.
		/// </summary>
		public bool   IsUnlocked       { get; private set; }
		/// <summary>
		/// Gets the total amount of science available for this experiment.
		/// </summary>
		public float  TotalScience     { get; private set; }
		/// <summary>
		/// Gets a value indicating whether all the science has been obtained for this experiment.
		/// This is science that has made it back to KSC
		/// </summary>
		public bool   IsComplete       { get; private set; }

		/// <summary>
		/// Completed Science + Onboard Science science
		/// </summary>
		public bool	IsCollected			{ get; private set; }

		/// <summary>
		/// Gets the amount of science for this experiment that is currently stored on vessels.
		/// </summary>
		public float OnboardScience		{ get; private set; }
		/// <summary>
		/// Gets the ScienceSubject containing information on how much science has been retrieved from this experiment.
		/// </summary>
		public ScienceSubject ScienceSubject { get; private set; }

		/// <summary>
		/// Gets the human-readable description of this experiment.
		/// </summary>
		public string Description {
			get {
				return string.Format(
					"{0} while {1}",
					ScienceExperiment.experimentTitle,
					Situation.Description);
			}
		}

		public string ShortDescription { get { return ScienceExperiment.experimentTitle; } }

		#endregion

		#region METHODS (PUBLIC)

		/// <summary>
		/// Updates the IsUnlocked, CompletedScience, TotalScience, OnboardScience, and IsComplete fields.
		/// </summary>
		/// <param name="onboardScience">The total onboard ScienceData.</param>
		public void Update( ScienceContext Sci )
		{
			ScienceSubject tmp;
			if( Sci.ScienceSubjects.TryGetValue( Id, out tmp ) )
				ScienceSubject = tmp;
			else
			{
					ScienceSubject = new ScienceSubject(ScienceExperiment, Situation.ExperimentSituation, Situation.Body.CelestialBody, Situation.SubBiome ?? Situation.Biome ?? string.Empty);
					Sci.ScienceSubjects.Add( Id, ScienceSubject );
			}
			
			IsUnlocked = Sci.UnlockedInstruments.IsUnlocked( ScienceExperiment.id ) && ( Situation.Body.Reached != null );

			CompletedScience = ScienceSubject.science * HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
			TotalScience = ScienceSubject.scienceCap * HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
			IsComplete = TotalScience - CompletedScience < 0.1 || NextScienceGain(ScienceExperiment, CompletedScience, TotalScience) < 0.1;

			OnboardScience = 0;
			List<ScienceData> data;
			if( Sci.OnboardScienceList.TryGetValue( ScienceSubject.id, out data ) )
				for( int x = 0; x < data.Count; x++ )
					OnboardScience += NextScienceGain(ScienceExperiment, CompletedScience + OnboardScience, TotalScience);

			var AllCollectedScience = CompletedScience + OnboardScience;
			IsCollected = TotalScience - AllCollectedScience < 0.1 || NextScienceGain(ScienceExperiment, AllCollectedScience, TotalScience) < 0.1;
		}
		#endregion

		#region METHODS (PRIVATE)
		/// <summary>
		/// Calculates science yield of next measurement.
		/// </summary>
		/// <param name="Experiment">The science experiment.</param>
		/// <param name="CollectedScience">The science collected so far.</param>
		/// <param name="TotalScience">The science cap.</param>
		static float NextScienceGain(ScienceExperiment Experiment, float CollectedScience, float TotalScience)
		{
			var RemainingScience = TotalScience - CollectedScience;
			var ScientificValue = Experiment.applyScienceScale ? 1 - (CollectedScience / TotalScience) : 1;
			var Multiplier = Experiment.baseValue / Experiment.scienceCap;
			return RemainingScience * ScientificValue * Multiplier;
		}
		#endregion
	}
}
