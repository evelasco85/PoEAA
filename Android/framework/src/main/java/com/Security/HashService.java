package com.Security;

import java.math.BigInteger;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

/**
 * Created by aiko on 3/4/17.
 */
public class HashService implements IHashService {
    private static IHashService ourInstance = new HashService();

    public static IHashService getInstance() {
        return ourInstance;
    }

    private HashService() {
    }

    public String ComputeHashValue(String content)
    {
        return bin2hex(StringToHashBytes(content));
    }

    static String bin2hex(byte[] data) {
        return String.format("%0" + (data.length*2) + "X", new BigInteger(1, data));
    }

    static byte[] StringToHashBytes(String value) {
        MessageDigest digest = null;
        try {
            digest = MessageDigest.getInstance("SHA-256");
        } catch (NoSuchAlgorithmException e1) {
            // TODO Auto-generated catch block
            e1.printStackTrace();
        }

        digest.reset();

        return digest.digest(value.getBytes());
    }
}
