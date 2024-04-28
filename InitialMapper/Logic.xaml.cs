namespace InitialMapper;

public partial class Logic : ContentPage
{
    TimeSpan travelTimeNoStops = new(4, 0, 0);
    TimeSpan travelTimeWithStops;
    public int numberOfStops;
    public int detourTimePerStop;
    
    public Logic()
	{
		InitializeComponent();
        stopFrequencyEntry.Completed += StopFrequencyEntry_Completed;
        detourTimeEntry.Completed += DetourTimeEntry_Completed;
	}

    private void DetourTimeEntry_Completed(object sender, EventArgs e)
    {
        detourTimePerStop = int.Parse(detourTimeEntry.Text);
        travelTimeWithStops = travelTimeNoStops.Add(new TimeSpan(0, detourTimePerStop * numberOfStops, 0));
        detourTimeLabel.Text = "Total trip time: " + travelTimeWithStops.Hours + " hours and " + travelTimeWithStops.Minutes + " minutes";
    }

    private void StopFrequencyEntry_Completed(object sender, EventArgs e)
    {
        numberOfStops = (int)(travelTimeNoStops.TotalMinutes / int.Parse(stopFrequencyEntry.Text));
        stopFrequencyLabel.Text = "Total number of stops: " + numberOfStops;
    }
}