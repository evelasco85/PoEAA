package CustomerServices;

import com.IdentityFieldAnnotation;
import com.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.Domain.DomainObject;

/**
 * Created by aiko on 3/5/17.
 */

public class Customer extends DomainObject {
    public Customer(IBaseMapper mapper)
    {
        super(mapper);
    }

    @IdentityFieldAnnotation
    public String Number;

    public String Name;
}
