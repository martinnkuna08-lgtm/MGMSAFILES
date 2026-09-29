package com.example.myapplication.Adapter;

import androidx.recyclerview.widget.RecyclerView;

import android.annotation.SuppressLint;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import android.widget.ImageView;

import com.example.myapplication.Model.AssignedDelay;
import com.example.myapplication.R;

import java.util.ArrayList;
import java.util.List;

public class DelayAdapter extends RecyclerView.Adapter<DelayAdapter.VH> {

    private List<AssignedDelay> list = new ArrayList<>();
    private final OnEditClickListener editListener;
    private final OnDeleteClickListener deleteListener;

    public DelayAdapter(OnEditClickListener editListener, OnDeleteClickListener deleteListener) {
        this.editListener = editListener;
        this.deleteListener = deleteListener;
    }

    public interface OnEditClickListener {
        void onEdit(int id);
    }

    public interface OnDeleteClickListener {
        void onDelete(int id);
    }

    public void setDelays(List<AssignedDelay> data) {
        this.list = data;
        notifyDataSetChanged();
    }

    @Override
    public VH onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater
                .from(parent.getContext())
                .inflate(R.layout.delay_adapter, parent, false);
        return new VH(v);
    }

    @SuppressLint("SetTextI18n")
    @Override
    public void onBindViewHolder(VH holder, int pos) {
        AssignedDelay d = list.get(pos);
        holder.tv.setText(
                "Delay ID: " + d.delayId + "\n" +
                        "Start Date Time: " + d.startDateTime + "\n" +
                        "End Date Time: " + d.endDateTime + "\n" +
                        "Duration: " + d.duration + "\n" +
                        "Heat Number: " + d.heatNo + "\n" +
                        "Discipline: " + d.discipline + "\n" +
                        "Reason: " + d.reason + "\n" +
                        "Operator Notes: " + d.operatorNotes + "\n" +
                        "Category: " + d.category
        );
        holder.ivEdit.setOnClickListener(v -> {
            if (d != null) {
                editListener.onEdit(d.id);
            }
        });
        holder.ivDelete.setOnClickListener(v -> {
            if (d != null) {
                deleteListener.onDelete(d.id);
            }
        });
    }

    @Override
    public int getItemCount() {
        return list.size();
    }

    static class VH extends RecyclerView.ViewHolder {
        TextView tv;
        ImageView ivEdit;
        ImageView ivDelete;

        VH(View itemView) {
            super(itemView);
            tv = itemView.findViewById(R.id.tvDelayInfo);
            ivEdit = itemView.findViewById(R.id.ivEdit);
            ivDelete = itemView.findViewById(R.id.ivDelete);
        }
    }
}
