//
//  LTCodecsJbigOptions.h
//  Leadtools.Codecs
//
//  Copyright (c) 1991-2020 LEAD Technologies, Inc. All rights reserved.
//

#import <Leadtools/LTPrimitives.h>

NS_ASSUME_NONNULL_BEGIN

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTCodecsJbigLoadOptions : NSObject

@property (nonatomic, assign) LeadSize resolution;

- (instancetype)init __unavailable;

@end

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

NS_CLASS_AVAILABLE(10_10, 8_0)
@interface LTCodecsJbigOptions : NSObject

@property (nonatomic, strong, readonly) LTCodecsJbigLoadOptions *load;

- (instancetype)init __unavailable;

@end

NS_ASSUME_NONNULL_END
