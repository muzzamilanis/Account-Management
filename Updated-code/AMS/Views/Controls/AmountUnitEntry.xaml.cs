using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AMS.Helpers;

namespace AMS.Views.Controls
{
    // Drop-in replacement for a plain amount TextBox: bind Value the same way you'd bind
    // Text="{Binding Model.Amount}" on a TextBox, and this shows a raw-number field plus a
    // magnitude dropdown (Units/Thousand/Lac/Million/Billion) beside it, so "2.5" + "Billion"
    // works instead of typing "2500000000" by hand.
    //
    // Always defaults to "Units (1)" — both on load and whenever Value is set from outside (e.g.
    // opening an existing record for edit) — so an existing number is never silently reinterpreted,
    // and not touching the dropdown never changes what was typed.
    public partial class AmountUnitEntry : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(double), typeof(AmountUnitEntry),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public static readonly DependencyProperty RawAmountProperty = DependencyProperty.Register(
            nameof(RawAmount), typeof(double), typeof(AmountUnitEntry),
            new PropertyMetadata(0.0, OnPartChanged));

        public static readonly DependencyProperty SelectedUnitProperty = DependencyProperty.Register(
            nameof(SelectedUnit), typeof(AmountUnitOption), typeof(AmountUnitEntry),
            new PropertyMetadata(AmountUnits.Options[0], OnPartChanged));

        public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
        public double RawAmount { get => (double)GetValue(RawAmountProperty); set => SetValue(RawAmountProperty, value); }
        public AmountUnitOption SelectedUnit { get => (AmountUnitOption)GetValue(SelectedUnitProperty); set => SetValue(SelectedUnitProperty, value); }
        public List<AmountUnitOption> UnitOptions { get; } = AmountUnits.Options.ToList();

        private bool _syncing;

        public AmountUnitEntry()
        {
            InitializeComponent();
        }

        private static void OnPartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (AmountUnitEntry)d;
            if (ctrl._syncing) return;
            ctrl._syncing = true;
            ctrl.Value = ctrl.RawAmount * (ctrl.SelectedUnit?.Multiplier ?? 1);
            ctrl._syncing = false;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (AmountUnitEntry)d;
            if (ctrl._syncing) return;
            ctrl._syncing = true;
            ctrl.RawAmount = (double)e.NewValue;
            ctrl.SelectedUnit = AmountUnits.Options[0];
            ctrl._syncing = false;
        }
    }
}
