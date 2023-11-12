package com.example.mystore.category;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;
import com.example.mystore.R;
import com.example.mystore.application.HomeApplication;
import com.example.mystore.contants.Urls;
import com.example.mystore.dto.category.CategoryItemDTO;

import java.util.List;

public class CategoriesAdapter extends RecyclerView.Adapter<CategoryCardViewHolder> {
    private List<CategoryItemDTO> categories;

    public CategoriesAdapter(List<CategoryItemDTO> categories) {
        this.categories = categories;
    }

    @NonNull
    @Override
    public CategoryCardViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.card_category_view, parent, false);
        return new CategoryCardViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull CategoryCardViewHolder holder, int position) {
        if(categories!=null && position<categories.size()) {
            CategoryItemDTO dto = categories.get(position);
            holder.getCategoryName().setText(dto.getName());
            Glide.with(HomeApplication.getAppContext())
                    .load(Urls.BASE + "/images/" + dto.getImage())
                    .apply(new RequestOptions().override(300))
                    .into(holder.getCategoryImage());
        }
    }

    @Override
    public int getItemCount() {
        return categories.size();
    }
}
