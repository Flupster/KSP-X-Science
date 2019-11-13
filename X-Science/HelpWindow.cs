using System;
using UnityEngine;
using KSP.Localization;



namespace ScienceChecklist
{
	class HelpWindow : Window<ScienceChecklistAddon>
	{
		private GUIStyle labelStyle;
		private GUIStyle sectionStyle;
		private Vector2 scrollPosition;
		private readonly ScienceChecklistAddon	_parent;



		public HelpWindow( ScienceChecklistAddon Parent )
			: base(Localizer.Format("#xScienceHelp_helpTitle"), 500, Screen.height * 0.75f  / Parent.Config.UiScale )//"[x] Science! Help"
		{
			_parent = Parent;
			UiScale = _parent.Config.UiScale;
			scrollPosition = Vector2.zero;
			_parent.Config.UiScaleChanged += OnUiScaleChange;
		}



		protected override void ConfigureStyles( )
		{
			base.ConfigureStyles();

			if( labelStyle == null )
			{
				labelStyle = new GUIStyle( _skin.label );
				labelStyle.wordWrap = true;
				labelStyle.fontStyle = FontStyle.Normal;
				labelStyle.normal.textColor = Color.white;
				labelStyle.stretchWidth = true;
				labelStyle.stretchHeight = false;
				labelStyle.margin.bottom -= wScale( 2 );
				labelStyle.padding.bottom -= wScale( 2 );
			}

			if( sectionStyle == null )
			{
				sectionStyle = new GUIStyle( labelStyle );
				sectionStyle.fontStyle = FontStyle.Bold;
			}
		}



		private void OnUiScaleChange( object sender, EventArgs e )
		{
			UiScale = _parent.Config.UiScale;
			labelStyle = null;
			sectionStyle = null;
			base.OnUiScaleChange( );
			ConfigureStyles( );
		}



		protected override void DrawWindowContents( int windowID )
		{
			scrollPosition = GUILayout.BeginScrollView( scrollPosition );
			GUILayout.BeginVertical( GUILayout.ExpandWidth( true ) );

			GUILayout.Label( "[x] Science! by Z-Key Aerospace and Bodrick.", sectionStyle, GUILayout.ExpandWidth( true ) );

			GUILayout.Space( wScale( 30 ) );
            GUILayout.Label(Localizer.Format("#xScienceHelp_helptext1"), sectionStyle, GUILayout.ExpandWidth(true));//"About"
            GUILayout.Label(Localizer.Format("#xScienceHelp_helptext2"), labelStyle, GUILayout.ExpandWidth(true));//"[x] Science! creates a list of all possible science.  Use the list to find what is possible, to see what is left to accomplish, to decide where your Kerbals are going next."

			GUILayout.Space( wScale( 20 ) );
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext3"), sectionStyle, GUILayout.ExpandWidth( true ) );//"The four filter buttons at the bottom of the window are"
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext4"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Show experiments available right now – based on you current ship and its situation"
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext5"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Show experiments available on this vessel – based on your ship but including all known biomes"
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext6"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Show all unlocked experiments – based on instruments you have unlocked and celestial bodies you have visited."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext7"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Show all experiments – shows everything.  You can hide this button"

			GUILayout.Space( wScale( 20 ) );
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext8"), sectionStyle, GUILayout.ExpandWidth( true ) );//"The text filter"
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext9"), labelStyle, GUILayout.ExpandWidth( true ) );//"To narrow your search, you may enter text into the filter eg \"kerbin’s shores\""
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext10"), labelStyle, GUILayout.ExpandWidth( true ) );//"Use – to mean NOT eg \"mun space -near\""
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext11"), labelStyle, GUILayout.ExpandWidth( true ) );//"Use | to mean OR eg \"mun|minmus space\""
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext12"), labelStyle, GUILayout.ExpandWidth( true ) );//"Hover the mouse over the \"123/456 completed\" text.  A pop-up will show more infromation."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext13"), labelStyle, GUILayout.ExpandWidth( true ) );//"Press the X button to clear your text filter."

			GUILayout.Space( wScale( 20 ) );
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext14"), sectionStyle, GUILayout.ExpandWidth( true ) );//"The settings are"
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext15"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Hide complete experiments – Any science with a full green bar is hidden.  It just makes it easier to see what is left to do."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext16"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Complete without recovery – Consider science in your spaceships as if it has been recovered.  You still need to recover to get the points.  It just makes it easier to see what is left to do."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext17"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Check debris – Science that survived a crash will be visible.  You may still be able to recover it."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext18"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Allow all filter – The \"All\" filter button shows science on planets you have never visited using instruments you have not invented yet.  Some people may consider it overpowered.  If you feel like a cheat, turn it off."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext19"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Filter difficult science – Hide science that is practically impossible.  Flying at stars, that kinda thing."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext20"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Use blizzy78's toolbar – If you have blizzy78’s toolbar installed then place the [x] Science! button on that instead of the stock \"Launcher\" toolbar."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext21"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Right click [x] icon – Choose to open the Here and Now window by right clicking.  Hides the second window.  Otherwise mute music."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext22"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Music starts muted – Music is muted on load."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext23"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Adjust UI Size – Change the scaling of the UI."

			GUILayout.Space( wScale( 20 ) );
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext24"), sectionStyle, GUILayout.ExpandWidth( true ) );//"Here and Now Window"
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext25"), labelStyle, GUILayout.ExpandWidth( true ) );//"The Here and Now Window will stop time-warp, display an alert message and play a noise when you enter a new situation.  To prevent this, close the window."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext26"), labelStyle, GUILayout.ExpandWidth( true ) );//"The Here and Now Window will show all outstanding experiments for the current situation that are possible with the current ship."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext27"), labelStyle, GUILayout.ExpandWidth( true ) );//"To run an experiment, click the button.  If the button is greyed-out then you may need to reset the experiment or recover or transmit the science."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext28"), labelStyle, GUILayout.ExpandWidth( true ) );//"To perform an EVA report or surface sample, first EVA your Kerbal.  The window will react, allowing those buttons to be clicked."

			GUILayout.Space( wScale( 20 ) );
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext29"), sectionStyle, GUILayout.ExpandWidth( true ) );//"Did you know? (includes spoilers)"
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext30"), labelStyle, GUILayout.ExpandWidth( true ) );//"* In the VAB editor you can use the filter \"Show experiments available on this vessel\" to see what your vessel could collect before you launch it."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext31"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Does the filter \"mun space high\" show mun’s highlands?  – use \"mun space –near\" instead."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext32"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Need more science?  Go to Minmus.  It’s a little harder to get to but your fuel will last longer.  A single mission can collect thousands of science points before you have to come back."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext33"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Generally moons are easier - it is more efficient to collect science from the surface of Ike or Gilly than from Duna or Eve.  That said - beware Tylo, it's big and you can't aerobrake."
			GUILayout.Label( Localizer.Format("#xScienceHelp_helptext34"), labelStyle, GUILayout.ExpandWidth( true ) );//"* Most of Kerbin’s biomes include both splashed and landed situations.  Landed at Kerbin’s water?  First build an aircraft carrier."

			GUILayout.EndVertical( );
			GUILayout.EndScrollView( );

			GUILayout.Space( wScale( 8 ) );
		}
	}
}
