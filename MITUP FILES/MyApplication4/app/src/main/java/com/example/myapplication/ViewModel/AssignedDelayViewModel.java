package com.example.myapplication.ViewModel;

import android.app.Application;

import androidx.annotation.NonNull;
import androidx.lifecycle.AndroidViewModel;

import com.example.myapplication.Model.AssignedDelay;
import com.example.myapplication.Data.AssignedDelayRepository;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import androidx.lifecycle.LiveData;

import java.util.List;

public class AssignedDelayViewModel extends AndroidViewModel {

    private final AssignedDelayRepository repository;
    private final ExecutorService executor = Executors.newSingleThreadExecutor();

    public AssignedDelayViewModel(@NonNull Application application) {
        super(application);
        repository = new AssignedDelayRepository(application);
    }

    public LiveData<List<AssignedDelay>> getAllDelays() {
        return repository.getAllDelays();
    }

    public LiveData<AssignedDelay> getDelayById(int id) {
        return repository.getDelayById(id);
    }

    public void insert(AssignedDelay delay) {
        executor.execute(() -> repository.insert(delay));
    }

    public void update(AssignedDelay delay) {
        executor.execute(() -> repository.update(delay));
    }

    public void delete(int id) {
        executor.execute(() -> repository.delete(id));
    }

    public void deleteAllDelays() {
        executor.execute(repository::deleteAllDelays);
    }
}
