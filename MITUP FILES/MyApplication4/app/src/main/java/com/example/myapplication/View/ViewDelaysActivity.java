package com.example.myapplication.View;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.RecyclerView;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.example.myapplication.Adapter.DelayAdapter;
import com.example.myapplication.ViewModel.AssignedDelayViewModel;

import androidx.lifecycle.ViewModelProvider;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;

import com.example.myapplication.R;

import androidx.appcompat.app.AlertDialog;


public class ViewDelaysActivity extends AppCompatActivity {
    private RecyclerView rvDelays;

    private DelayAdapter adapter;
    private AssignedDelayViewModel viewModel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_view_delays);

        setupRecycler();
        setupViewModel();
        setupInsertButton();
        setupDeleteButton();
    }

    private void setupRecycler() {
        rvDelays = findViewById(R.id.rvDelays);
        rvDelays.setLayoutManager(new LinearLayoutManager(this));

        adapter = new DelayAdapter(
                id -> {
                    // Edit click
                    Intent intent = new Intent(ViewDelaysActivity.this, EditDelaysActivity.class);
                    intent.putExtra("id", id);
                    startActivity(intent);
                },
                id -> {
                    new AlertDialog.Builder(ViewDelaysActivity.this)
                            .setTitle("Delete Delay")
                            .setMessage("Do you want to delete this delay?")
                            .setPositiveButton("Yes", (dialog, which) -> {
                                viewModel.delete(id); // Make sure your ViewModel has deleteById(int id)
                            })
                            .setNegativeButton("No", null)
                            .show();
                }
        );
        rvDelays.setAdapter(adapter);
    }

    // Observe LiveData outside onCreate
    private void setupViewModel() {
        viewModel = new ViewModelProvider(this).get(AssignedDelayViewModel.class);
        viewModel.getAllDelays().observe(this, delays -> {
            adapter.setDelays(delays);
        });
    }

    private void setupInsertButton() {
       Button btnInsertDelays = findViewById(R.id.btnInsertDelays);
        btnInsertDelays.setOnClickListener(v -> {
            Intent intent = new Intent(ViewDelaysActivity.this, MainActivity.class);
            startActivity(intent);
        });
    }
    private void setupLoginButton() {
        Button btnLogin = findViewById(R.id.btnLogin);
        btnLogin.setOnClickListener(v -> {
            Intent intent = new Intent(ViewDelaysActivity.this, LoginActivity.class);
            startActivity(intent);
        });
    }
    private void setupDeleteButton() {
        Button btnDeleteAll = findViewById(R.id.btnDeleteDelays);
        btnDeleteAll.setOnClickListener(v -> {
            new AlertDialog.Builder(ViewDelaysActivity.this)
                    .setTitle("Delete All Delays")
                    .setMessage("Are you sure you want to delete all delays?")
                    .setPositiveButton("Yes", (dialog, which) -> viewModel.deleteAllDelays())
                    .setNegativeButton("No", null)
                    .show();
        });

    }

}
