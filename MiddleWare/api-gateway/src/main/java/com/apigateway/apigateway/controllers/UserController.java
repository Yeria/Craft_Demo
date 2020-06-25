package com.apigateway.apigateway.controllers;

import java.util.concurrent.CompletableFuture;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.web.client.RestTemplateBuilder;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.scheduling.annotation.Async;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

import models.NetworthRequest;
import models.User;

@RestController
public class UserController {
	private final RestTemplate restTemplate;
    private static final Logger log = LoggerFactory.getLogger(UserController.class);
    
    public UserController(RestTemplateBuilder restTemplateBuilder) {
    	this.restTemplate = restTemplateBuilder.build();
    }
    
    @Autowired
    private String apiUrl;
    
    @Async
    @CrossOrigin(origins = "http://localhost:4200")
    @GetMapping("/getuser")
    public CompletableFuture<Object> calculateNetworth(@RequestParam int id, @RequestHeader HttpHeaders headers) {
		HttpEntity request = new HttpEntity(headers);
    	String url = apiUrl + "/v1/user/getuser?id=" + id;
    	ResponseEntity<User> response = restTemplate.exchange(url, HttpMethod.GET, request, User.class);
        return CompletableFuture.completedFuture(response);
    }
}
