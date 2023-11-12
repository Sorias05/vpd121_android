package com.example.mystore.service;

import com.example.mystore.contants.Urls;
import com.example.mystore.network.CategoriesApi;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class ApplicationNetwork {
    private static ApplicationNetwork instance;
    private Retrofit retrofit;

    public ApplicationNetwork() {
        retrofit = new Retrofit
                .Builder()
                .baseUrl(Urls.BASE)
                .addConverterFactory(GsonConverterFactory.create())
                .build();
    }
    public static ApplicationNetwork getInstance() {
        if(instance==null)
            instance=new ApplicationNetwork();
        return instance;
    }
    public CategoriesApi getCategoriesApi() {
        return retrofit.create(CategoriesApi.class);
    }
}
