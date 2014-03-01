namespace eu.Vanaheimr.Nyi
{

    using Microsoft.SPOT.Hardware;

    /// <summary>
    /// Specifies the various pull-up resistor types.
    /// </summary>
    public enum PullUpResistor
    {

        /// <summary>
        /// A value that represents an external pull-up resistor.
        /// </summary>
        External = Port.ResistorMode.Disabled,

        /// <summary>
        /// A value that represents an internal pull-up resistor.
        /// </summary>
        Internal = Port.ResistorMode.PullUp

    }

}