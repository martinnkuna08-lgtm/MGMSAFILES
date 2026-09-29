package com.example.myapplication.View;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.lifecycle.ViewModelProvider;

import com.example.myapplication.Model.AssignedDelay;
import com.example.myapplication.R;
import com.example.myapplication.ViewModel.AssignedDelayViewModel;


// MainActivity.java
public class MainActivity extends AppCompatActivity {

    private EditText[] inputs;
    private Button btnInsert, btnViewDelays;
    private AssignedDelayViewModel viewModel;

    private final int[] editTextIds = {
            R.id.etDelayId, R.id.etStartDateTime, R.id.etEndDateTime, R.id.etDuration,
            R.id.etCategory, R.id.etHeatNo, R.id.etDiscipline, R.id.etReason, R.id.etOperatorNotes
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        inputs = new EditText[editTextIds.length];
        for (int i = 0; i < editTextIds.length; i++) {
            inputs[i] = findViewById(editTextIds[i]);
        }


        setBtnInsertDelays();
        setBtnViewDelays();
        viewModel = new ViewModelProvider(this).get(AssignedDelayViewModel.class);
    }

    private void setBtnInsertDelays() {
        btnInsert = findViewById(R.id.btnInsert);
        btnInsert.setOnClickListener(v -> insertDelayEntry());
    }

    private void setBtnViewDelays() {

        btnViewDelays = findViewById(R.id.btnViewDelays);
        btnViewDelays.setOnClickListener(v -> {
            Intent intent = new Intent(MainActivity.this, ViewDelaysActivity.class);
            startActivity(intent);
        });

    }


    private void insertDelayEntry() {

        String[] values = new String[inputs.length];

        // Collect values and validate non-empty
        for (int i = 0; i < inputs.length; i++) {
            values[i] = inputs[i].getText().toString().trim();
            if (values[i].isEmpty()) {
                String fieldName = getResources().getResourceEntryName(editTextIds[i]);
                showToast("Please fill in the fields");
                return;
            }
        }

        AssignedDelay delayEntry = new AssignedDelay();
        delayEntry.delayId = Integer.parseInt(values[0]);
        delayEntry.startDateTime = values[1];
        delayEntry.endDateTime = values[2];
        delayEntry.duration = values[3];
        delayEntry.category = values[4];
        delayEntry.heatNo = values[5];
        delayEntry.discipline = values[6];
        delayEntry.reason = values[7];
        delayEntry.operatorNotes = values[8];

        viewModel.insert(delayEntry);
        showToast("Delay Entry Inserted");
        clearFields();


    }

    private void clearFields() {
        for (EditText et : inputs) {
            et.setText("");
        }
    }

    private void showToast(String msg) {
        Toast.makeText(this, msg, Toast.LENGTH_SHORT).show();
    }
}

