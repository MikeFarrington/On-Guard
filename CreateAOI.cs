﻿using SAAI.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SAAI
{


  /// <summary>
  /// A dialog for creating and AreaOfInterest
  /// </summary>
  public partial class CreateAOI : Form
  {
    public AreaOfInterest Area { get; set; }
    Rectangle _rectangle;
    public int OriginalXResolution { get; set; }
    public int OriginalYResolution { get; set; }

    public bool DeleteItem { get; set; }


    public CreateAOI(Rectangle imageRect, Point zoneFocus, int xResolution, int yResolution) // The area on the actual image, not the display image
    {
      InitializeComponent();
      Area = new AreaOfInterest
      {
        ZoneFocus = zoneFocus,
        OriginalXResolution = xResolution,
        OriginalYResolution = yResolution,
      };

      _rectangle = imageRect;
      doorButton.Checked = true;
      anyActivityButton.Checked = true;

      if (imageRect.X < -5000)
      {
        imageRect.X = -5000;
      }

      if (imageRect.Y < -5000)
      {
        imageRect.Y = -5000;
      }

      xNumeric.Value = imageRect.X;
      yNumeric.Value = imageRect.Y;
      widthNumeric.Value = imageRect.Width;
      heighNumeric.Value = imageRect.Height;
      OriginalXResolution = xResolution;
      OriginalYResolution = yResolution;


      if (!Settings.Default.EmailSetup)
      {
        MessageBox.Show("In order to set Areas of Interest you must first set your email contact information.  This is a one time only requirement.");
        using (OutgoingEmailDialog dlg = new OutgoingEmailDialog())
        {
          dlg.ShowDialog();
        }
      }

    }

    public CreateAOI(AreaOfInterest area) // The area on the actual image, not the display image
    {
      InitializeComponent();
      Area = area;
      _rectangle = area.AreaRect;
      Area.ZoneFocus = area.ZoneFocus;
      Rectangle rect = area.AreaRect;
      doorButton.Checked = true;
      anyActivityButton.Checked = true;
      OriginalXResolution = area.OriginalXResolution;
      OriginalYResolution = area.OriginalYResolution;

      if (rect.X < -5000)
      {
        rect.X = -5000;
      }

      if (rect.Y < -5000)
      {
        rect.Y = -5000;
      }


      xNumeric.Value = rect.X;
      yNumeric.Value = rect.Y;
      widthNumeric.Value = rect.Width;
      heighNumeric.Value = rect.Height;

      aoiNameText.Text = area.AOIName;

      switch (area.AOIType)
      {
        case AOIType.Door:
          doorButton.Checked = true;
          break;

        case AOIType.PeopleWalking:
          peopleWalkingButton.Checked = true;
          break;

        case AOIType.GarageDoor:
          garageButton.Checked = true;
          break;

        case AOIType.Driveway:
          drivewayButton.Checked = true;
          break;

        case AOIType.IgnoreObjects:
          ignoreButton.Checked = true;
          break;
      }

      switch (area.MovementType)
      {
        case MovementType.AnyActivity:
          anyActivityButton.Checked = true;
          break;

        case MovementType.Arrival:
          arrivingButton.Checked = true;
          break;

        case MovementType.Departure:
          departingButton.Checked = true;
          break;
      }

      if (null != area.SearchCriteria)
      {
        foreach (ObjectCharacteristics obj in area.SearchCriteria)
        {
          switch (obj.ObjectType)
          {
            case ImageObjectType.People:
              peopleCheck.Checked = true;
              peopleConfidenceNumeric.Value = obj.Confidence;
              peopleMinimumOverlap.Value = obj.MinPercentOverlap;
              peopleFramesNumeric.Value = obj.TimeFrame;
              peopleMinXNumeric.Value = obj.MinimumXSize;
              peopleMinYNumeric.Value = obj.MinimumYSize;
              break;

            case ImageObjectType.Cars:
              carsCheck.Checked = true;
              carsConfidenceNumeric.Value = obj.Confidence;
              carsOverlapNumeric.Value = obj.MinPercentOverlap;
              carsFramesNumeric.Value = obj.TimeFrame;
              carsMinXNumeric.Value = obj.MinimumXSize;
              carsMinYNumeric.Value = obj.MinimumYSize;
              break;

            case ImageObjectType.Motorcycles:
              motorcycleCheck.Checked = true;
              motorcyclesConfidenceNumeric.Value = obj.Confidence;
              motorcyclesOverlapNumeric.Value = obj.MinPercentOverlap;
              motorcyclesFramesNumeric.Value = obj.TimeFrame;
              motorcyclesMinXNumeric.Value = obj.MinimumXSize;
              motorcyclesMinYNumeric.Value = obj.MinimumYSize;
              break;

            case ImageObjectType.Trucks:
              truckCheck.Checked = true;
              trucksConfidenceNumeric.Value = obj.Confidence;
              trucksOverlapNumeric.Value = obj.MinPercentOverlap;
              trucksFramesNumeric.Value = obj.TimeFrame;
              trucksMinXNumeric.Value = obj.MinimumXSize;
              trucksMinYNumeric.Value = obj.MinimumYSize;
              break;

            case ImageObjectType.Bikes:
              bikeCheck.Checked = true;
              bikesConfidenceNumeric.Value = obj.Confidence;
              bikesOverlapNumeric.Value = obj.MinPercentOverlap;
              bikesFramesNumeric.Value = obj.TimeFrame;
              bikesMinXNumeric.Value = obj.MinimumXSize;
              bikesMinYNumeric.Value = obj.MinimumYSize;
              break;

            case ImageObjectType.Bears:
              bearsCheck.Checked = true;
              bearsConfidenceNumeric.Value = obj.Confidence;
              bearsOverlapNumeric.Value = obj.MinPercentOverlap;
              bearsFramesNumeric.Value = obj.TimeFrame;
              bearsMinXNumeric.Value = obj.MinimumXSize;
              bearsMinYNumeric.Value = obj.MinimumXSize;
              break;

            case ImageObjectType.Animals:
              animalsCheck.Checked = true;
              animalsConfidenceNumeric.Value = obj.Confidence;
              animalsOverlapNumeric.Value = obj.MinPercentOverlap;
              animalsFramesNumeric.Value = obj.TimeFrame;
              animalsMinXNumeric.Value = obj.MinimumXSize;
              animalsMinYNumeric.Value = obj.MinimumXSize;
              break;

          }
        }
      }

    }

    private bool SaveAreaData()
    {
      bool result = true;
      if (string.IsNullOrEmpty(aoiNameText.Text))
      {
        MessageBox.Show("You must provide a name for this area!");
        result = false;
      }
      else
      {
        if (Area.SearchCriteria != null)
        {
          Area.SearchCriteria.Clear();
        }

        Area.AOIName = aoiNameText.Text;

        if (doorButton.Checked)
        {
          Area.AOIType = AOIType.Door;
        }
        else if (garageButton.Checked)
        {
          Area.AOIType = AOIType.GarageDoor;
        }
        else if (drivewayButton.Checked)
        {
          Area.AOIType = AOIType.Driveway;
        }
        else if (peopleWalkingButton.Checked)
        {
          Area.AOIType = AOIType.PeopleWalking;
        }
        else if (ignoreButton.Checked)
        {
          Area.AOIType = AOIType.IgnoreObjects;
        }

        if (anyActivityButton.Checked)
        {
          Area.MovementType = MovementType.AnyActivity;
        }
        else if (arrivingButton.Checked)
        {
          Area.MovementType = MovementType.Arrival;
        }
        else if (departingButton.Checked)
        {
          Area.MovementType = MovementType.Departure;
        }

        if (peopleCheck.Checked)
        {
          ObjectCharacteristics c = new ObjectCharacteristics()
          {
            ObjectType = ImageObjectType.People,
            Confidence = (int)peopleConfidenceNumeric.Value,
            MinPercentOverlap = (int)peopleMinimumOverlap.Value,
            TimeFrame = (int)peopleFramesNumeric.Value,
            MinimumXSize = (int)peopleMinXNumeric.Value,
            MinimumYSize = (int)peopleMinYNumeric.Value,


          };
          Area.SearchCriteria.Add(c);
        }

        if (carsCheck.Checked)
        {
          ObjectCharacteristics c = new ObjectCharacteristics
          {
            ObjectType = ImageObjectType.Cars,
            Confidence = (int)carsConfidenceNumeric.Value,
            MinPercentOverlap = (int)carsOverlapNumeric.Value,
            TimeFrame = (int)carsFramesNumeric.Value,
            MinimumXSize = (int)carsMinXNumeric.Value,
            MinimumYSize = (int)carsMinYNumeric.Value,

          };
          Area.SearchCriteria.Add(c);
        }

        if (truckCheck.Checked)
        {
          ObjectCharacteristics c = new ObjectCharacteristics
          {
            ObjectType = ImageObjectType.Trucks,
            Confidence = (int)trucksConfidenceNumeric.Value,
            MinPercentOverlap = (int)trucksOverlapNumeric.Value,
            TimeFrame = (int)trucksFramesNumeric.Value,
            MinimumXSize = (int)trucksMinXNumeric.Value,
            MinimumYSize = (int)trucksMinYNumeric.Value,
          };
          Area.SearchCriteria.Add(c);
        }

        if (motorcycleCheck.Checked)
        {
          ObjectCharacteristics c = new ObjectCharacteristics
          {
            ObjectType = ImageObjectType.Motorcycles,
            Confidence = (int)motorcyclesConfidenceNumeric.Value,
            MinPercentOverlap = (int)motorcyclesOverlapNumeric.Value,
            TimeFrame = (int)motorcyclesFramesNumeric.Value,
            MinimumXSize = (int)motorcyclesMinXNumeric.Value,
            MinimumYSize = (int)motorcyclesMinYNumeric.Value,
          };
          Area.SearchCriteria.Add(c);
        }

        if (bikeCheck.Checked)
        {
          ObjectCharacteristics c = new ObjectCharacteristics
          {
            ObjectType = ImageObjectType.Bikes,
            Confidence = (int)bikesConfidenceNumeric.Value,
            MinPercentOverlap = (int)bikesOverlapNumeric.Value,
            TimeFrame = (int)bikesFramesNumeric.Value,
            MinimumXSize = (int)bikesMinXNumeric.Value,
            MinimumYSize = (int)bikesMinYNumeric.Value,
          };
          Area.SearchCriteria.Add(c);
        }

        if (bearsCheck.Checked)
        {
          ObjectCharacteristics c = new ObjectCharacteristics
          {
            ObjectType = ImageObjectType.Bears,
            Confidence = (int)bearsConfidenceNumeric.Value,
            MinPercentOverlap = (int)bearsOverlapNumeric.Value,
            TimeFrame = (int)bearsFramesNumeric.Value,
            MinimumXSize = (int)bearsMinXNumeric.Value,
            MinimumYSize = (int)bearsMinYNumeric.Value,
          };
          Area.SearchCriteria.Add(c);
        }

        if (animalsCheck.Checked)
        {
          ObjectCharacteristics c = new ObjectCharacteristics
          {
            ObjectType = ImageObjectType.Animals,
            Confidence = (int)animalsConfidenceNumeric.Value,
            MinPercentOverlap = (int)animalsOverlapNumeric.Value,
            TimeFrame = (int)animalsFramesNumeric.Value,
            MinimumXSize = (int)animalsMinXNumeric.Value,
            MinimumYSize = (int)animalsMinYNumeric.Value,
          };
          Area.SearchCriteria.Add(c);
        }

        _rectangle = new Rectangle((int)xNumeric.Value, (int)yNumeric.Value, (int)widthNumeric.Value, (int)heighNumeric.Value);
        Area.AreaRect = _rectangle;
        Area.OriginalXResolution = BitmapResolution.XResolution;
        Area.OriginalYResolution = BitmapResolution.YResolution;
      }

      return result;

    }

    private void OKButton_Click(object sender, EventArgs e)
    {
      if (SaveAreaData())
      {
        DialogResult = DialogResult.OK;
        Close();
      }
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void DeleteAOIButton_Click(object sender, EventArgs e)
    {
      DeleteItem = true;
      Close();
    }

    private void NotificationsButton_Click(object sender, EventArgs e)
    {
      using (NotificationOptionsDialog dlg = new NotificationOptionsDialog(Area))
      {
        dlg.ShowDialog(this);
        DialogResult = DialogResult.None;
      }
    }

    private void AreaAdjustButton_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(this, "Adjusting an area size/shape automatically saves any changes.  Proceed?", "Adjusting Area Size/Shape", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        if (SaveAreaData())
        {
          DialogResult = DialogResult.Yes;
          Close();
        }
        else
        {
          DialogResult = DialogResult.None;
        }
      }
      else
      {
        DialogResult = DialogResult.None;
      }
    }
  }
}
