package com.example.myapplication.Model;

import androidx.room.Entity;
import androidx.room.PrimaryKey;
import androidx.annotation.NonNull;

@Entity(tableName = "Assigned_Delays")
public class AssignedDelay {

    @PrimaryKey(autoGenerate = true)
    public int id;
    public int delayId;

    public String startDateTime = "";

    public String endDateTime = "";

    public String duration = "";

    public String category = "";

    public String heatNo = "";

    public String discipline = "";

    public String reason = "";

    public String operatorNotes = "";


}
