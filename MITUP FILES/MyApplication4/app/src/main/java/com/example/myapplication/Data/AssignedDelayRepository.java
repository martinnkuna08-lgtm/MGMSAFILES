package com.example.myapplication.Data;

import android.app.Application;

import com.example.myapplication.Model.AssignedDelay;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import androidx.lifecycle.LiveData;

import java.util.List;

import android.util.Log;

public class AssignedDelayRepository {

    private AssignedDelayDao delayDao;
    private ExecutorService executorService;

    public AssignedDelayRepository(Application application) {
        AppDatabase db = AppDatabase.getDatabase(application);
        delayDao = db.assignedDelayDao();
        executorService = Executors.newSingleThreadExecutor();
    }

    public LiveData<List<AssignedDelay>> getAllDelays() {
        return delayDao.getAllDelays();
    }

    public LiveData<AssignedDelay> getDelayById(int id) {
        return delayDao.getDelayById(id);
    }

    public void insert(AssignedDelay delay) {
        executorService.execute(() -> {
            long count = delayDao.insert(delay);
            Log.d("AssignedDelayRepo", "Inserted " + count + " row(s)");
        });
    }

    public void update(AssignedDelay delay) {
        executorService.execute(() -> {
            int count = delayDao.update(delay);
            Log.d("AssignedDelayRepo", "Updated " + count + " row(s)");
        });
    }

    public void delete(int id) {
        executorService.execute(() -> {
            int count = delayDao.delete(id);
            Log.d("AssignedDelayRepo", "Deleted " + count + " row(s) by ID");
        });
    }

    public void deleteAllDelays() {
        executorService.execute(() -> {
            int count = delayDao.deleteAllDelays();
            Log.d("Repo", "Deleted " + count + " delays");
        });
    }

}