using System;
using UnityEngine;
using KSP.Localization;



namespace ScienceChecklist
{
	class SettingsWindow : Window<ScienceChecklistAddon>
	{
		private readonly string version;
		private GUIStyle labelStyle;
		private GUIStyle toggleStyle;
		private GUIStyle sliderStyle;
		private GUIStyle editStyle;
		private GUIStyle versionStyle;
		private GUIStyle selectionStyle;

		private readonly Logger _logger;
		private readonly ScienceChecklistAddon _parent;



		// Constructor
		public SettingsWindow( ScienceChecklistAddon Parent )
			: base( Localizer.Format("#xScienceSetting_title"), 240, 360 )//"[x] Science! Settings"
		{
			_logger = new Logger( this );
			_parent = Parent;
			UiScale = 1; // Don't let this change
			version = Utilities.GetDllVersion( this );
		}


		// For our Window base class
		protected override void ConfigureStyles( )
		{
			base.ConfigureStyles( );

			if( labelStyle == null )
			{
				labelStyle = new GUIStyle( _skin.label );
				labelStyle.wordWrap = false;
				labelStyle.fontStyle = FontStyle.Normal;
				labelStyle.normal.textColor = Color.white;

				toggleStyle = new GUIStyle( _skin.toggle );
				sliderStyle = new GUIStyle( _skin.horizontalSlider );
				editStyle = new GUIStyle( _skin.textField );
				versionStyle = Utilities.GetVersionStyle( );
				selectionStyle = new GUIStyle( _skin.button );
				selectionStyle.margin = new RectOffset( 30, 0, 0, 0 );
			}
		}



		// For our Window base class
		protected override void DrawWindowContents( int windowID )
		{
			GUILayout.BeginVertical( );

			bool save = false;
			var toggle = GUILayout.Toggle( _parent.Config.HideCompleteExperiments, new GUIContent( Localizer.Format("#xScienceSetting_Hidecomplete"), Localizer.Format("#xScienceSetting_Hidecomplete_tooltip") ), toggleStyle );//"Hide complete experiments""Experiments considered complete will not be shown."
			if( toggle != _parent.Config.HideCompleteExperiments )
			{
				_parent.Config.HideCompleteExperiments = toggle;
				save = true;
			}

			toggle = GUILayout.Toggle( _parent.Config.CompleteWithoutRecovery, new GUIContent( Localizer.Format("#xScienceSetting_CompleteWithoutRecovery"), Localizer.Format("#xScienceSetting_CompleteWithoutRecovery_tooltip") ), toggleStyle );//"Complete without recovery""Show experiments as completed even if they have not been recovered yet.\nYou still need to recover the science to get the points!\nJust easier to see what is left."
			if( toggle != _parent.Config.CompleteWithoutRecovery )
			{
				_parent.Config.CompleteWithoutRecovery = toggle;
				save = true;
			}

			toggle = GUILayout.Toggle(_parent.Config.CheckUnloadedVessels, new GUIContent(Localizer.Format("#xScienceSetting_CheckUnloadedVessels"), Localizer.Format("#xScienceSetting_CheckUnloadedVessels_tooltip")), toggleStyle);//"Check unloaded vessels""Unloaded vessels will be checked for recoverable science."
			if( toggle != _parent.Config.CheckUnloadedVessels )
			{
				_parent.Config.CheckUnloadedVessels = toggle;
				save = true;
			}

			toggle = GUILayout.Toggle( _parent.Config.CheckDebris, new GUIContent( Localizer.Format("#xScienceSetting_Checkdebris"), Localizer.Format("#xScienceSetting_Checkdebris_tooltip") ), toggleStyle );//"Check debris""Vessels marked as debris will be checked for recoverable science."
			if( toggle != _parent.Config.CheckDebris )
			{
				_parent.Config.CheckDebris = toggle;
				save = true;
			}

			toggle = GUILayout.Toggle( _parent.Config.AllFilter, new GUIContent( Localizer.Format("#xScienceSetting_AllFilter"), Localizer.Format("#xScienceSetting_AllFilter_tooltip") ), toggleStyle );//"Allow all filter""Adds a filter button showing all experiments, even on unexplored bodies using unavailable instruments.\nMight be considered cheating."
			if( toggle != _parent.Config.AllFilter )
			{
				_parent.Config.AllFilter = toggle;
				save = true;
			}

			toggle = GUILayout.Toggle( _parent.Config.FilterDifficultScience, new GUIContent( Localizer.Format("#xScienceSetting_FilterDifficultScience"), Localizer.Format("#xScienceSetting_FilterDifficultScience_tooltip") ), toggleStyle );//"Filter difficult science""Hide a few experiments such as flying at stars and gas giants that are almost impossible.\n Also most EVA reports before upgrading Astronaut Complex."
			if( toggle != _parent.Config.FilterDifficultScience )
			{
				_parent.Config.FilterDifficultScience = toggle;
				save = true;
			}

			toggle = GUILayout.Toggle( _parent.Config.SelectedObjectWindow, new GUIContent( Localizer.Format("#xScienceSetting_SelectedObjectWindow"), Localizer.Format("#xScienceSetting_SelectedObjectWindow_tooltip") ), toggleStyle );//"Selected Object Window""Show the Selected Object Window in the Tracking Station."
			if( toggle != _parent.Config.SelectedObjectWindow )
			{
				_parent.Config.SelectedObjectWindow = toggle;
				save = true;
			}

			if( BlizzysToolbarButton.IsAvailable )
			{
				toggle = GUILayout.Toggle( _parent.Config.UseBlizzysToolbar, new GUIContent( Localizer.Format("#xScienceSetting_UseBlizzysToolbar"), Localizer.Format("#xScienceSetting_UseBlizzysToolbar_tooltip") ), toggleStyle );//"Use blizzy78's toolbar""Remove [x] Science button from stock toolbar and add to blizzy78 toolbar."
				if( toggle != _parent.Config.UseBlizzysToolbar )
				{
					_parent.Config.UseBlizzysToolbar = toggle;
					save = true;
				}
			}

			GUILayout.Space(2);
			int selected = 0;
			if( !_parent.Config.RighClickMutesMusic )
				selected = 1;
			int new_selected = selected;
			GUILayout.Label( Localizer.Format("#xScienceSetting_RighClick"), labelStyle );//"Right click [x] icon"
			GUIContent[] Options = {
				new GUIContent( Localizer.Format("#xScienceSetting_Mutemusic"), Localizer.Format("#xScienceSetting_Mutemusic_desc") ),//"Mute music""Here & Now window gets its own icon"
				new GUIContent( Localizer.Format("#xScienceSetting_OpensHereandNow"), Localizer.Format("#xScienceSetting_OpensHereandNow_desc") )//"Opens Here & Now window""Here & Now icon is hidden"
			};
			new_selected = GUILayout.SelectionGrid( selected, Options, 1, selectionStyle );
			if( new_selected != selected )
			{
				if( new_selected == 0 )
					_parent.Config.RighClickMutesMusic = true;
				else
					_parent.Config.RighClickMutesMusic = false;
				save = true;
			}

			if( _parent.Config.RighClickMutesMusic )
			{
				toggle = GUILayout.Toggle( _parent.Config.MusicStartsMuted, new GUIContent( Localizer.Format("#xScienceSetting_MutesMusic"), Localizer.Format("#xScienceSetting_MutesMusic_desc") ), toggleStyle );//"Music starts muted""Title music is silenced upon load.  No need to right click"
				if( toggle != _parent.Config.MusicStartsMuted )
				{
					_parent.Config.MusicStartsMuted = toggle;
					save = true;
				}
			}

			GUILayout.Space(2);
			GUILayout.Label(new GUIContent( Localizer.Format("#xScienceSetting_AdjustUIsize"), Localizer.Format("#xScienceSetting_AdjustUIsize_desc") ), labelStyle );//"Adjust UI size""Adjusts the the UI scaling."
			var slider = GUILayout.HorizontalSlider( _parent.Config.UiScale, 1, 2 );
			if( slider != _parent.Config.UiScale )
			{
				_parent.Config.UiScale = slider;
				save = true;
			}

			if( save )
				_parent.Config.Save( );

			GUILayout.EndVertical( );
			GUILayout.Space(10);
			GUI.Label( new Rect( 4, windowPos.height - 13, windowPos.width - 20, 12 ), "[x] Science! V" + version, versionStyle );//
		}
	}
}
