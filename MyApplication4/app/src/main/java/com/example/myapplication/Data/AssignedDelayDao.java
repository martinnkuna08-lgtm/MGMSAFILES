package com.example.myapplication.Data;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Update;
import androidx.room.Delete;
import androidx.room.Query;
import androidx.lifecycle.LiveData;

import java.util.List;

import com.example.myapplication.Model.AssignedDelay;

@Dao
public interface AssignedDelayDao {

    @Insert
    long insert(AssignedDelay delay);


    @Update
    int update(AssignedDelay delay);

    @Query("DELETE FROM Assigned_Delays WHERE id = :id")
    int delete(int id);


    @Query("SELECT * FROM Assigned_Delays ORDER BY id DESC")
    LiveData<List<AssignedDelay>> getAllDelays();

    @Query("SELECT * FROM Assigned_Delays WHERE id = :id")
    LiveData<AssignedDelay> getDelayById(int id);

    @Query("DELETE FROM Assigned_Delays")
    int deleteAllDelays();

}
