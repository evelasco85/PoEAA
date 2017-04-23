package com.Interfaces;

import java.lang.reflect.Field;
import java.util.List;

/**
 * Created by aiko on 3/4/17.
 */

public interface IIdentityMap {
    List<Field> GetIdentityFields();
    Boolean EntityHasValidIdentityFields();
    void ClearEntities();
    int GetCount();
}
