package com.example.myapplication.View;

import androidx.appcompat.app.AppCompatActivity;

import android.widget.EditText;
import android.widget.Button;

import com.example.myapplication.Model.AssignedDelay;
import com.example.myapplication.ViewModel.AssignedDelayViewModel;

import android.os.Bundle;

import com.example.myapplication.R;

import androidx.lifecycle.ViewModelProvider;


public class EditDelaysActivity extends AppCompatActivity {
    private EditText etDelayId, etStartDateTime, etEndDateTime, etDuration, etHeatNo,
            etDiscipline, etReason, etOperatorNotes, etCategory;
    private Button btnSave;
    private AssignedDelayViewModel viewModel;
    private int delayId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_edit_delays);

        delayId = getIntent().getIntExtra("id", -1);

        setupViews();
        setupViewModel();
        setupSaveButton();
    }

    private void setupViews() {
        etDelayId = findViewById(R.id.etDelayId);
        etStartDateTime = findViewById(R.id.etStartDateTime);
        etEndDateTime = findViewById(R.id.etEndDateTime);
        etDuration = findViewById(R.id.etDuration);
        etHeatNo = findViewById(R.id.etHeatNo);
        etDiscipline = findViewById(R.id.etDiscipline);
        etReason = findViewById(R.id.etReason);
        etOperatorNotes = findViewById(R.id.etOperatorNotes);
        etCategory = findViewById(R.id.etCategory);
        btnSave = findViewById(R.id.btnSave);
    }

    private void setupViewModel() {
        if (delayId == -1) {
            finish(); // invalid ID
            return;
        }
        viewModel = new ViewModelProvider(this).get(AssignedDelayViewModel.class);
        viewModel.getDelayById(delayId).observe(this, delay -> {
            if (delay != null) populateFields(delay);
        });
    }

    private void populateFields(AssignedDelay delay) {
        etDelayId.setText(String.valueOf(delay.delayId));
        etStartDateTime.setText(delay.startDateTime);
        etEndDateTime.setText(delay.endDateTime);
        etDuration.setText(delay.duration);
        etHeatNo.setText(delay.heatNo);
        etDiscipline.setText(delay.discipline);
        etReason.setText(delay.reason);
        etOperatorNotes.setText(delay.operatorNotes);
        etCategory.setText(delay.category);

    }

    private void setupSaveButton() {
        btnSave.setOnClickListener(v -> saveDelay());
    }

    private void saveDelay() {
        AssignedDelay updated = new AssignedDelay();
        updated.id = delayId;
        updated.delayId = Integer.parseInt(etDelayId.getText().toString());
        updated.startDateTime = etStartDateTime.getText().toString();
        updated.endDateTime = etEndDateTime.getText().toString();
        updated.duration = etDuration.getText().toString();
        updated.category = etCategory.getText().toString();
        updated.heatNo = etHeatNo.getText().toString();
        updated.discipline = etDiscipline.getText().toString();
        updated.reason = etReason.getText().toString();
        updated.operatorNotes = etOperatorNotes.getText().toString();
        viewModel.update(updated);
        finish();
    }
}
